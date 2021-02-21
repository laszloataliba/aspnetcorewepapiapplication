using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Business.Services
{
    public class SupplierService : BaseService, ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IAddressRepository _addressRepository;

        public SupplierService(
                ISupplierRepository supplierRepository,
                IAddressRepository addressRepository,
                INotifier notifier) :
            base(notifier)
        {
            _supplierRepository = supplierRepository;
            _addressRepository = addressRepository;
        }

        public async Task<bool> Add(Supplier supplier)
        {
            if (
                    (!ExecuteValidation(new SupplierValidation(), supplier))
                        ||
                    (!ExecuteValidation(new AddressValidation(), supplier.Address))
               ) return false;

            if (_supplierRepository.Get(sup => sup.IdentificationNumber == supplier.IdentificationNumber).Result.Any())
            {
                Notify("Já existe um fornecedor com este documento informado.");
                return false;
            }

            await _supplierRepository.Add(supplier);

            return true;
        }

        public async Task<bool> Update(Supplier supplier)
        {
            if (!ExecuteValidation(new SupplierValidation(), supplier)) return false;

            if (_supplierRepository.Get(sup => sup.IdentificationNumber == supplier.IdentificationNumber && sup.Id != supplier.Id).Result.Any())
            {
                Notify("Já existe um fornecedor com este documento informado.");
                return false;
            }

            await _supplierRepository.Update(supplier);

            return true;
        }

        public async Task UpdateAddress(Address address)
        {
            if (!ExecuteValidation(new AddressValidation(), address)) return;

            await _addressRepository.Update(address);
        }

        public async Task<bool> Remove(Guid id)
        {
            if (_supplierRepository.GetSupplierProductsAddress(id).Result.Products.Any())
            {
                Notify("O fornecedor possui produtos cadastrados!");
                return false;
            }

            var address = await _addressRepository.GetAddressBySupplier(id);

            if (address != null)
                await _addressRepository.Remove(address.Id);

            await _supplierRepository.Remove(id);

            return true;
        }

        public void Dispose()
        {
            _supplierRepository?.Dispose();
            _addressRepository?.Dispose();
        }
    }
}
