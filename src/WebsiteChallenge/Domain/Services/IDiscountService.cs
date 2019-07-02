using Domain.Discounts;
using Domain.Entities;
using Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IDiscountService
    {
        Task<IEnumerable<Discount>> GetDiscounts(IEnumerable<LineItem> lineItems);
    }
}