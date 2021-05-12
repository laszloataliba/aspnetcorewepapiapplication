using DevIO.Api.Controllers;
using DevIO.Api.Extensions;
using DevIO.Api.ViewModels;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Api.V1.Controllers
{
    [ApiVersion("1.0"),
     Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;

        public AuthController(
            INotifier notifier,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<AppSettings> appSettings,
            IUser user,
            ILogger<AuthController> logger
        ) : base(notifier, user)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        [HttpPost("new-account")]
        public async Task<IActionResult> Register(RegisterUserViewModel registerUserViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUserViewModel.Email,
                Email = registerUserViewModel.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUserViewModel.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);

                return CustomResponse(await GenerateToken(user.Email));
            }

            foreach (var error in result.Errors)
            {
                NotifyError(error.Description);
            }

            return CustomResponse(registerUserViewModel);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserViewModel loginUserViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await 
                _signInManager.PasswordSignInAsync(
                        loginUserViewModel.Email, 
                        loginUserViewModel.Password, 
                        false, 
                        true
                    );

            if (result.Succeeded)
            {
                _logger.LogInformation($"Usuário {loginUserViewModel.Email} logado!");
                return CustomResponse(await GenerateToken(loginUserViewModel.Email));
            }

            if (result.IsLockedOut)
            {
                NotifyError("Usuário temporariamente bloqueado por tentativas inválidas.");
                return CustomResponse(loginUserViewModel);
            }

            NotifyError("Usuário e/ou senha incorretos.");

            return CustomResponse(loginUserViewModel);
        }

        private async Task<LoginResponseViewModel> GenerateToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token =
                tokenHandler.CreateToken(
                    new SecurityTokenDescriptor
                    {
                        Issuer = _appSettings.Emitter,
                        Audience = _appSettings.ValidIn,
                        Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationInHours),
                        Subject = identityClaims,

                        SigningCredentials =
                            new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    }
                );

            var encodedToken = tokenHandler.WriteToken(token);

            var response =
                new LoginResponseViewModel
                {
                    AccessToken = encodedToken,
                    ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationInHours).TotalSeconds,

                    UserToken =
                        new UserTokenViewModel
                        {
                            Id = user.Id,
                            Email = user.Email,

                            Claims = 
                                claims.Select(claim => 
                                    new ClaimViewModel
                                    {
                                        Type = claim.Type,
                                        Value = claim.Value
                                    }
                                )
                        }
                };

            return response;
        }

        private static long ToUnixEpochDate(DateTime date) => 
            (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}