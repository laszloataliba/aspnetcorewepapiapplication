using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(MyDbContext context) : 
            base(context)
        {
        }

        public async Task<Supplier> GetSupplierAddress(Guid id)
        {
            return await Db.Suppliers.AsNoTracking()
                .Include(address => address.Address)
                    .FirstOrDefaultAsync(supplier => supplier.Id == id);
        }

        public async Task<Supplier> GetSupplierProductsAddress(Guid id)
        {
            return await Db.Suppliers.AsNoTracking()
                .Include(product => product.Products)
                    .Include(address => address.Address)
                        .FirstOrDefaultAsync(supplier => supplier.Id == id);
        }
    }
}
