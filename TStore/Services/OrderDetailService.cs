using TStore.Common;
using TStore.Data;
using TStore.Interfaces;
using TStore.Models;

namespace TStore.Services
{
    public class OrderDetailService : BaseService<OrderDetail>, IOrderDetailService
    {
        public OrderDetailService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
