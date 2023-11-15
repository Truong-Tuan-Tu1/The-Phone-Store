﻿using TStore.Common;

namespace TStore.Models
{
    public class CartItem : BaseModel
    {
        public int CartId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public Cart Cart { get; set; } = null!;

        public Product Product { get; set; } = null!;
    }
}
