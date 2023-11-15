using TStore.Common;
using TStore.Data;
using TStore.Interfaces;
using TStore.Models;

namespace TStore.Services;

public class SupplierService : BaseService<Supplier>, ISupplierService
{
    public SupplierService(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}