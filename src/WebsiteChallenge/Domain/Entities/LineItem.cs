using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class LineItem
    {
        public Product Product { get; set; }
        public string Name => Product.ProductType.ToString();
        public int Quantity { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Subtotal
        {
            get { return (Product.Price * Quantity) - DiscountAmount; }
        }
    }
}
