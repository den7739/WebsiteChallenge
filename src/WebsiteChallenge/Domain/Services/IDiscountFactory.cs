using Domain.Discounts;
using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface IDiscountFactory
    {
        Discount GetDiscount(ProductType type);
    }
}