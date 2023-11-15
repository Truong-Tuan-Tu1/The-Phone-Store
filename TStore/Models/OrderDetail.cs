using System.ComponentModel.DataAnnotations;
using TStore.Common;

namespace TStore.Models;

public class OrderDetail : BaseModel
{
    [Key]
    public int OrderId { get; set; }

    public string ProductName { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }

    public Orders Orders { get; set; } = null!;
}