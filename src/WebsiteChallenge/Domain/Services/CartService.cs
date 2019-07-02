using Domain.Entities;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        public void AddItem(Guid cartId, LineItem item)
        {
            var cart = cartRepository.GetById(cartId);
            if (cart != null)
            {
                if (cart.LineItems.Any() && cart.LineItems.FirstOrDefault(x => x.Product?.Id == item.Product.Id) != null)
                {
                    cart.LineItems.FirstOrDefault(x => x.Product.Id == item.Product.Id).Quantity++;
                }
                else
                {
                    cart.LineItems.Add(item);
                    
                }
            }
            else {
                cart = new Cart
                {
                    Id = cartId,
                    DateCreated = DateTime.Now,
                    LineItems = new List<LineItem> { item }
                };
            }
            cartRepository.AddOrUpdate(cart);
        }

        public void RemoveLine(Guid cartId, LineItem item, bool isWholeLine)
        {
            var cart = cartRepository.GetById(cartId);
            if (cart == null)
            {
                return;
            }
            if (isWholeLine)
            {
                cart.LineItems.Remove(item);
            }
            else {
                cart.LineItems.First(x => x.Product.Id == item.Product.Id).Quantity--;
            }
            cartRepository.AddOrUpdate(cart);
        }

        public void Clear(Guid cartId)
        {
            var cart = cartRepository.GetById(cartId);
            if (cart == null)
            {
                return;
            }
            cartRepository.Clear(cart);
        }

        public Cart GetCart(Guid cartId) => cartRepository.GetById(cartId);
    }
}
