using TStore.Models;

namespace TStore.Interfaces;

public interface IProductCategoryService : IBaseService<ProductCategory>
{
    public bool Create(ProductCategory entity, IFormFile file);


}