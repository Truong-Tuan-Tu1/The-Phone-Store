using TStore.Common;
using TStore.Data;
using TStore.Interfaces;
using TStore.Models;

namespace TStore.Services
{
    public class OrdersService : BaseService<Orders>, IOrdersService
    {
        public OrdersService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
