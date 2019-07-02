using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;

namespace Domain.Repository
{
    public class CartRepository : ICartRepository
    {
        static List<Cart> cartList = new List<Cart>();


        public Cart GetById(Guid cartId)
        {
            return cartList.First(x => x.Id == cartId);
        }

        public void AddOrUpdate(Cart cart)
        {
            var existingCart = cartList.First(x => x.Id == cart.Id);
            if (existingCart != null)
            {
                cartList.Remove(existingCart);
                cartList.Add(cart);
            }
            else {
                cartList.Add(cart);
            }
        }

        public void Clear(Cart cart)
        {
            cartList.RemoveAll(x => x.Id == cart.Id);
        }
    }
}
