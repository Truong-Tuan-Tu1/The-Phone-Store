using TStore.Common;

namespace TStore.Models;

public class Orders : BaseModel
{
    public DateTime OrderDate { get; set; }
    public string CustomerId { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}