using AutoMapper;
using DevIO.Api.Controllers;
using DevIO.Api.Extensions;
using DevIO.Api.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.Api.V1.Controllers
{
    [ApiVersion("1.0"),
     Route("api/v{version:apiVersion}/[controller]"), 
     Authorize]
    public class SuppliersController : MainController
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;
        private readonly ISupplierService _supplierService;
        private readonly IAddressRepository _addressRepository;

        public SuppliersController(
            ISupplierRepository supplierRepository,
            IMapper mapper,
            ISupplierService supplierService,
            INotifier notifier,
            IAddressRepository addressRepository,
            IUser user) : base(notifier, user)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
            _supplierService = supplierService;
            _addressRepository = addressRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var supplier = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAll());

            return Ok(supplier);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var supplier = await GetSupplierProductsAddress(id);

            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        [HttpPost, 
         ClaimsAuthorizer("Supplier", "Add")]
        public async Task<ActionResult<SupplierViewModel>> Add(SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            await _supplierService.Add(_mapper.Map<Supplier>(supplierViewModel));

            return CustomResponse(supplierViewModel);
        }

        [HttpPut("{id:guid}"), 
         ClaimsAuthorizer("Supplier", "Update")]
        public async Task<ActionResult<SupplierViewModel>> Update(Guid id, SupplierViewModel supplierViewModel)
        {
            if (id != supplierViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query.");
                return CustomResponse(supplierViewModel);
            }

            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            await _supplierService.Update(_mapper.Map<Supplier>(supplierViewModel));

            return CustomResponse(supplierViewModel);
        }

        [HttpDelete("{id:guid}"), 
         ClaimsAuthorizer("Supplier", "Remove")]
        public async Task<ActionResult<SupplierViewModel>> Delete(Guid id)
        {
            var supplierViewModel = await GetSupplierAddress(id);

            if (supplierViewModel == null)
                return NotFound();

            await _supplierService.Remove(id);

            return CustomResponse();
        }

        [HttpGet("get-address/{id:guid}")]
        public async Task<AddressViewModel> GetAddressById(Guid id)
        {
            return _mapper.Map<AddressViewModel>(await _addressRepository.GetById(id));
        }

        [HttpPut("update-address/{id:guid}"), 
         ClaimsAuthorizer("Supplier", "Update")]
        public async Task<IActionResult> UpdateAddress(Guid id, AddressViewModel addressViewModel)
        {
            if (id != addressViewModel.Id)
            {
                NotifyError("O id informado não é o mesmo que foi passado na query.");
                return CustomResponse(addressViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _supplierService.UpdateAddress(_mapper.Map<Address>(addressViewModel));

            return CustomResponse(addressViewModel);
        }

        private async Task<SupplierViewModel> GetSupplierProductsAddress(Guid id)
        {
            return _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierProductsAddress(id));
        }

        private async Task<SupplierViewModel> GetSupplierAddress(Guid id)
        {
            return _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierAddress(id));
        }
    }
}
