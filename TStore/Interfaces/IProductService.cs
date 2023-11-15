using TStore.Models;

namespace TStore.Interfaces;

public interface IProductService : IBaseService<Product>
{
    public List<Product> SearchProduct(int? categoryId, int? brandId, int? supplierId, string name);
    public List<Product> SearchAllProduct(string key);
    public List<Product> FindProductByCategoryId(int? categoruId);
    public int UpdateActionHotProduct(int id, bool isHot);
}