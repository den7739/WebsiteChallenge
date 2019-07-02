using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services
{
    public interface ICartService
    {
        Cart GetCart(Guid cartId);
        void AddItem(Guid cartId, LineItem item);

        void RemoveLine(Guid cartId, LineItem item, bool isWholeLine);

        void Clear(Guid cartId);
    }
}
