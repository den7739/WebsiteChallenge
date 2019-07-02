using System;
using Domain.Entities;
using MediatR;

namespace Domain.Handlers
{
    public class RemoveLineItemRequest : IRequest
    {
        public bool IsWholeLine { get; set; }
        public Guid CartId { get; set; }
        public LineItem Item { get; set; }
    }
}