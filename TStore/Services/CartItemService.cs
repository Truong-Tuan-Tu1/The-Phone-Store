using TStore.Common;
using TStore.Data;
using TStore.Interfaces;
using TStore.Models;

namespace TStore.Services
{
    public class CartItemService : BaseService<CartItem>, ICartItemService
    {
        private readonly ApplicationDbContext _dbContext;

        public CartItemService(ApplicationDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public bool UpdateQuantity(int id, int quantity)
        {
            var cartItem = base.FindById(id);
            if (cartItem == null)
            {
                return false;
            }

            cartItem.Quantity = quantity;
            return base.Update(cartItem);


        }
    }
}
