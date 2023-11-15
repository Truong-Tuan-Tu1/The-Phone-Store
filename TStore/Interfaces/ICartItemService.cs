using TStore.Models;

namespace TStore.Interfaces
{
    public interface ICartItemService : IBaseService<CartItem>
    {
        public bool UpdateQuantity(int id, int quantity);
    }
}
