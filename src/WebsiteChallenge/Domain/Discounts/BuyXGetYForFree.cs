using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;

namespace Domain.Discounts
{
    public class BuyXGetYForFree : Discount
    {
        public BuyXGetYForFree(string name, IList<ProductType> applicableProductTypes, int minAmountOfItems, ProductType freeItemType, int amountOfFreeItems)
            : base(name)
        {
            ApplicableProductTypes = applicableProductTypes;
            MinAmountOfItems = minAmountOfItems;
            AmountOfFreeItems = amountOfFreeItems;
            FreeItemType = freeItemType;
        }

        public override Cart ApplyDiscount()
        {
            // custom processing
            if (Cart.LineItems.Any(x => ApplicableProductTypes.Contains(x.Product.ProductType) && x.Quantity >= MinAmountOfItems))
            {
                var product = new Product { ProductType = FreeItemType }; // should get from repository
                Cart.LineItems.Add(new LineItem
                {
                    Product = product,
                    Quantity = AmountOfFreeItems,
                    DiscountAmount = product.Price * AmountOfFreeItems
                });
            }
            return Cart;
        }

        public virtual IList<ProductType> ApplicableProductTypes { get; set; }
        public virtual ProductType FreeItemType { get; set; }
        public virtual int AmountOfFreeItems { get; set; }
        public virtual int MinAmountOfItems { get; set; }
    }
}
