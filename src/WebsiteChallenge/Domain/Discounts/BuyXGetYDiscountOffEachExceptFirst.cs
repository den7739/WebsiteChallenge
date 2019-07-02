using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Domain.Discounts
{
    public class BuyXGetYDiscountOffEachExceptFirst : Discount
    {
        public BuyXGetYDiscountOffEachExceptFirst(string name, IList<ProductType> applicableProductTypes, decimal discountPercentage, int minAmountOfItems)
            : base(name)
        {
            DiscountPercentage = discountPercentage;
            ApplicableProductTypes = applicableProductTypes;
            MinAmountOfItems = minAmountOfItems;
        }

        public override Cart ApplyDiscount()
        {
            // custom processing
            foreach (var lineItem in Cart.LineItems)
            {
                if (ApplicableProductTypes.Contains(lineItem.Product.ProductType) && lineItem.Quantity >= MinAmountOfItems)
                {
                    var quantity = lineItem.Quantity - 1;
                    lineItem.DiscountAmount = (lineItem.Product.Price * quantity) * DiscountPercentage;
                }
            }
            return Cart;
        }

        public virtual IList<ProductType> ApplicableProductTypes { get; set; }
        public virtual decimal DiscountPercentage { get; set; }
        public virtual int MinAmountOfItems { get; set; }
    }
}
