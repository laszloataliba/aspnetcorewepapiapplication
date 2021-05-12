using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

namespace DevIO.Api.Extensions
{
    public class CustomAuthorizer
    {
        public static bool ValidateUserClaims(HttpContext context, string claimName, string claimValue)
        {
            return (
                context.User.Identity.IsAuthenticated
                    &&
                context.User.Claims.Any(claim => claim.Type == claimName && claim.Value.Contains(claimValue))
            );
        }
    }

    public class ClaimsAuthorizerAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizerAttribute(string claimName, string claimValue) : 
            base(typeof(RequisiteClaimFilter))
        {
            Arguments = new object[] { new Claim(claimName, claimValue) };
        }
    }

    public class RequisiteClaimFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;

        public RequisiteClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            if (!CustomAuthorizer.ValidateUserClaims(context.HttpContext, _claim.Type, _claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
