using Domain.Entities;
using System;

namespace Domain.Repository
{
    public interface ICartRepository
    {
        Cart GetById(Guid cartId);
        void AddOrUpdate(Cart cart);
        void Clear(Cart cart);
    }
}