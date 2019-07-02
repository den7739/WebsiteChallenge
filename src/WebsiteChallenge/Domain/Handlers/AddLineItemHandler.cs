using Domain.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public class AddLineItemHandler : AsyncRequestHandler<AddLineItemRequest>
    {
        private readonly ICartService cartService;

        public AddLineItemHandler(ICartService cartService)
        {
            this.cartService = cartService;
        }

        protected override Task Handle(AddLineItemRequest request, CancellationToken cancellationToken)
        {
            cartService.AddItem(request.CartId, request.Item);
            return Task.CompletedTask;
        }
    }
}
