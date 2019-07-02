using Domain.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public class RemoveLineItemHandler : AsyncRequestHandler<RemoveLineItemRequest>
    {
        private readonly ICartService cartService;

        public RemoveLineItemHandler(ICartService cartService)
        {
            this.cartService = cartService;
        }

        protected override Task Handle(RemoveLineItemRequest request, CancellationToken cancellationToken)
        {
            cartService.RemoveLine(request.CartId, request.Item, request.IsWholeLine);
            return Task.CompletedTask;
        }
    }
}
