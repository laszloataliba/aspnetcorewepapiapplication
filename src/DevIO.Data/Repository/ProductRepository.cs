using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MyDbContext context) : 
            base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsBySupplier(Guid id)
        {
            return await Get(product => product.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsSuppliers()
        {
            return await Db.Products.AsNoTracking()
                .Include(supplier => supplier.Supplier)
                    .OrderBy(product => product.Name)
                        .ToListAsync();
        }

        public async Task<Product> GetProductSupplier(Guid id)
        {
            return await Db.Products.AsNoTracking()
                .Include(supplier => supplier.Supplier)
                    .FirstOrDefaultAsync(product => product.Id == id);
        }
    }
}
