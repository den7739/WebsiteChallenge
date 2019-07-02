using System;
using MediatR;

namespace Domain.Handlers
{
    public class ClearCartRequest : IRequest
    {
        public Guid CartId { get; set; }
    }
}