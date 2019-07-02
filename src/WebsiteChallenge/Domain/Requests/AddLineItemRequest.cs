using System;
using Domain.Entities;
using MediatR;

namespace Domain.Handlers
{
    public class AddLineItemRequest : IRequest
    {
        public LineItem Item { get; set; }
        public Guid CartId { get; set; }
    }
}