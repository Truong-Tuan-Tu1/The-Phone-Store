using TStore.Common;

namespace TStore.Models;

public class Brand : BaseModel
{
    public string Name { get; set; }
    public string? SeoName { get; set; }

    public ICollection<Product> Products { get; } = new List<Product>();
}