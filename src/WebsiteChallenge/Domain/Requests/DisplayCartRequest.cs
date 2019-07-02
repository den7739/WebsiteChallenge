using System;
using Domain.ViewModels;
using MediatR;

namespace Domain.Handlers
{
    public class DisplayCartRequest : IRequest<CartViewModel>
    {
        public Guid CartId { get; set; }
    }
}