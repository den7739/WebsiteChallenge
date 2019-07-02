using Domain.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Handlers
{
    public class ClearCartHandler : AsyncRequestHandler<ClearCartRequest>
    {
        private readonly ICartService cartService;

        public ClearCartHandler(ICartService cartService)
        {
            this.cartService = cartService;
        }

        protected override Task Handle(ClearCartRequest request, CancellationToken cancellationToken)
        {
            cartService.Clear(request.CartId);
            return Task.CompletedTask;
        }
    }
}
