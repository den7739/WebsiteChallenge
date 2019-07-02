using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Discounts
{
    public class BuyXGetYDiscountOffWhole : Discount
    {
        public BuyXGetYDiscountOffWhole(string name, IList<ProductType> applicableProductTypes, decimal discountPercentage, int minAmountOfItems)
            : base(name)
        {
            DiscountPercentage = discountPercentage;
            ApplicableProductTypes = applicableProductTypes;
            MinAmountOfItems = minAmountOfItems;
        }
        public override Cart ApplyDiscount()
        {
            // custom processing
            if (Cart.LineItems.Any(x => ApplicableProductTypes.Contains(x.Product.ProductType) && x.Quantity >= MinAmountOfItems))
            {
                foreach (var lineItem in Cart.LineItems)
                {
                    lineItem.DiscountAmount += lineItem.Product.Price * DiscountPercentage * lineItem.Quantity;
                }
            }
            return Cart;
        }

        public virtual IList<ProductType> ApplicableProductTypes { get; set; }
        public virtual decimal DiscountPercentage { get; set; }
        public virtual int MinAmountOfItems { get; set; }
    }
}
