using System;
using System.Collections.Generic;
using System.Text;
using Domain.Discounts;
using Domain.Entities;

namespace Domain.Services
{
    public class DiscountFactory : IDiscountFactory
    {
        public Discount GetDiscount(ProductType type)
        {
            var availableDiscount = new Dictionary<ProductType, Discount>();
            availableDiscount.Add(ProductType.Bags, new BuyXGetYDiscountOffEachExceptFirst(
                "Buy 2 or more Bags of Pogs and get 50% off each bag (excluding the first one).", new List<ProductType> { ProductType.Bags }, 0.5m, 2));
            availableDiscount.Add(ProductType.LargeBowl, new BuyXGetYForFree(
                "Buy a Large bowl of Trifle and get a free Paper Mask.", new List<ProductType> { ProductType.LargeBowl }, 1, ProductType.PaperMask, 1));
            availableDiscount.Add(ProductType.Shurikens, new BuyXGetYDiscountOffWhole(
                "Buy 100 or more Shurikens and get 30% off whole basket.", new List<ProductType> { ProductType.Shurikens }, 0.3m, 100));
            return availableDiscount[type];
        }
    }
}
