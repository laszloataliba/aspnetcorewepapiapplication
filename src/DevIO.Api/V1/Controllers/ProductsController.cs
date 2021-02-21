using AutoMapper;
using DevIO.Api.Controllers;
using DevIO.Api.Extensions;
using DevIO.Api.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DevIO.Api.V1.Controllers
{
    [ApiVersion("1.0"),
     Route("api/v{version:apiVersion}/[controller]"), 
     Authorize]
    public class ProductsController : MainController
    {
        private readonly INotifier _notifier;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController
        (
            INotifier notifier,
            IProductRepository productRepository,
            IProductService productService,
            IMapper mapper,
            IUser user
        ) : base(notifier, user)
        {
            _productRepository = productRepository;
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet, 
         AllowAnonymous]
        public async Task<IEnumerable<ProductViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsSuppliers());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductViewModel>> GetById(Guid id)
        {
            var productViewModel = await GetProduct(id);

            if (productViewModel == null) return NotFound();

            return productViewModel;
        }

        [HttpPost, 
         ClaimsAuthorizer("Product", "Add")]
        public async Task<ActionResult<ProductViewModel>> Add(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imageName = $"{Guid.NewGuid()}_{productViewModel.Name}";

            if (!FileUpload(productViewModel.Image, imageName))
                return CustomResponse(productViewModel);

            productViewModel.Image = imageName;

            await _productService.Add(_mapper.Map<Product>(productViewModel));

            return CustomResponse(productViewModel);
        }

        [HttpPost("AddLargeImage"), 
         ClaimsAuthorizer("Product", "Add")]
        public async Task<ActionResult<ProductImageViewModel>> Add(ProductImageViewModel productViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imagePrefix = Guid.NewGuid().ToString();

            if (!await LargeFileUpload(productViewModel.ImageUpload, imagePrefix))
                return CustomResponse(ModelState);

            productViewModel.Name = $"{imagePrefix}_{productViewModel.ImageUpload.FileName}";

            await _productService.Add(_mapper.Map<Product>(productViewModel));

            return CustomResponse(productViewModel);
        }

        [HttpPut("{id:guid}"), 
         ClaimsAuthorizer("Product", "Update")]
        public async Task<IActionResult> Update(Guid id, ProductImageViewModel productViewModel)
        {
            if (id != productViewModel.Id)
            {
                NotifyError("Os ids informados não são iguais!");
                return NotFound();
            }

            var productUpdate = await GetProduct(id);
            productViewModel.Name = productUpdate.Name;

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (productViewModel.Image != null)
            {
                var imageName = $"{Guid.NewGuid().ToString()}_{productViewModel.Name}";

                if (!FileUpload(productViewModel.Image, imageName))
                    return CustomResponse(ModelState);

                //productUpdate.Name = imageName;
            }

            productUpdate.Name = productViewModel.Name;
            productUpdate.Description = productViewModel.Description;
            productUpdate.Value = productViewModel.Value;
            productUpdate.Active = productViewModel.Active;

            await _productService.Update(_mapper.Map<Product>(productUpdate));

            return CustomResponse(productViewModel);
        }

        [RequestSizeLimit(40000000),
         HttpPost("AddLargeFile")]
        public async Task<IActionResult> Add(IFormFile file)
        {
            return Ok();
        }

        [HttpDelete("{id:guid}"), 
         ClaimsAuthorizer("Product", "Remove")]
        public async Task<ActionResult<ProductViewModel>> Delete(Guid id)
        {
            var productViewModel = await GetProduct(id);

            if (productViewModel == null) return NotFound();

            await _productService.Remove(id);

            return CustomResponse(productViewModel);
        }

        private async Task<ProductViewModel> GetProduct(Guid id)
        {
            return _mapper.Map<ProductViewModel>(await _productRepository.GetProductSupplier(id));
        }

        private async Task<bool> LargeFileUpload(IFormFile file, string imagePrefix)
        {
            if (file == null || file.Length == 0)
            {
                NotifyError("Forneça uma imagem para este produto.");
                return false;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", $"{imagePrefix}_{imagePrefix}");

            if (System.IO.File.Exists(filePath))
            {
                NotifyError("Já existe um arquivo com este nome.");
                return false;
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return true;
        }

        private bool FileUpload(string file, string imageName)
        {
            var imageDataByteArray = Convert.FromBase64String(file);

            if (String.IsNullOrEmpty(file))
            {
                NotifyError("Forneça uma imagem para este produto.");
                return false;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);

            if (System.IO.File.Exists(filePath))
            {
                NotifyError("Já existe um arquivo com este nome.");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }
    }
}
