using TStore.Common;
using TStore.Data;
using TStore.Interfaces;
using TStore.Models;

namespace TStore.Services
{
    public class CartService : BaseService<Cart>, ICartService
    {
        public CartService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
