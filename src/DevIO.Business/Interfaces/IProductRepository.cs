using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsBySupplier(Guid id);
        Task<IEnumerable<Product>> GetProductsSuppliers();
        Task<Product> GetProductSupplier(Guid id);
    }
}
