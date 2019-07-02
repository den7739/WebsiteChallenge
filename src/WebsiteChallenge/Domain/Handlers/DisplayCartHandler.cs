using Domain.Repository;
using Domain.Services;
using Domain.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public class DisplayCartHandler : IRequestHandler<DisplayCartRequest, CartViewModel>
    {
        private readonly ICartRepository cartRepository;
        private readonly IDiscountService discountService;

        public DisplayCartHandler(ICartRepository cartRepository, IDiscountService discountService)
        {
            this.cartRepository = cartRepository;
            this.discountService = discountService;
        }

        public async Task<CartViewModel> Handle(DisplayCartRequest request, CancellationToken cancellationToken)
        {
            var cart = cartRepository.GetById(request.CartId);
            var discounts = await discountService.GetDiscounts(cart.LineItems);
            cart.AddDiscount(discounts);
            var lineItemViewModels = cart.LineItems.Select(lineItem =>
                new LineItemViewModel
                {
                    ProductName = lineItem.Product.Name,
                    ProductType = lineItem.Product.ProductType,
                    Count = lineItem.Quantity,
                    SubTotal = lineItem.Subtotal
                }).ToList();
            var discountViewModels = discounts.Select(x => new DiscountViewModel { Name = x.Name }).ToList();
            return new CartViewModel
            {
                LineItems = lineItemViewModels,
                Discounts = discountViewModels,
                DiscountTotal = cart.LineItems.Sum(x => x.DiscountAmount),
                CartSubTotal = cart.LineItems.Sum(x => x.Subtotal) + cart.LineItems.Sum(x => x.DiscountAmount),
                Total = cart.LineItems.Sum(x => x.Subtotal)
            };
        }
    }
}
