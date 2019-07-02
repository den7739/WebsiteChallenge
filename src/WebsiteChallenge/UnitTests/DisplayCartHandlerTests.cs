using Domain.Discounts;
using Domain.Entities;
using Domain.Handlers;
using Domain.Repository;
using Domain.Services;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class DisplayCartHandlerTests
    {
        [TestMethod]
        public async Task Should_Display_Cart_With_Discount()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            var cartId = Guid.NewGuid();
            Cart expected = GetExpectedValidCart(cartId);
            IEnumerable<Discount> discounts = GetExpectedDiscounts();
            var mockCartRepository = new Mock<ICartRepository>();
            var mockDiscountService = new Mock<IDiscountService>();
            CartService cart = new CartService(mockCartRepository.Object);
            mockCartRepository.Setup(m => m.GetById(cartId))
                .Returns(expected);
            mockDiscountService.Setup(x => x.GetDiscounts(expected.LineItems)).ReturnsAsync(discounts);

            var displayCartHandler = new DisplayCartHandler(mockCartRepository.Object, mockDiscountService.Object);

            // Act
            var result = await displayCartHandler.Handle(new DisplayCartRequest { CartId = cartId }, new System.Threading.CancellationToken());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.LineItems.Count(), expected.LineItems.Count);
            Assert.AreEqual(result.DiscountTotal, 142.3m);
            Assert.AreEqual(result.CartSubTotal, 441m);
            Assert.AreEqual(result.Total, 298.7m);
            Assert.AreEqual(result.Discounts.Count(), 3);
        }

        [TestMethod]
        public async Task Should_Display_Cart_WithOut_Discount()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            var cartId = Guid.NewGuid();
            Cart expected = GetExpectedInValidCart(cartId);
            IEnumerable<Discount> discounts = new List<Discount>();
            var mockCartRepository = new Mock<ICartRepository>();
            var mockDiscountService = new Mock<IDiscountService>();
            CartService cart = new CartService(mockCartRepository.Object);
            mockCartRepository.Setup(m => m.GetById(cartId))
                .Returns(expected);
            mockDiscountService.Setup(x => x.GetDiscounts(expected.LineItems)).ReturnsAsync(discounts);

            var displayCartHandler = new DisplayCartHandler(mockCartRepository.Object, mockDiscountService.Object);

            // Act
            var result = await displayCartHandler.Handle(new DisplayCartRequest { CartId = cartId }, new System.Threading.CancellationToken());

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.LineItems.Count(), expected.LineItems.Count);
            Assert.AreEqual(result.DiscountTotal, 0m);
            Assert.AreEqual(result.CartSubTotal, 24m);
            Assert.AreEqual(result.Total, 24m);
            Assert.AreEqual(result.Discounts.Count(), 0);
        }

        private Cart GetExpectedInValidCart(Guid cartId)
        {
            Product product2 = new Product { Id = "2", Name = "Product2", Description = "Test Desc2", Price = 4, ProductType = ProductType.Shurikens };
            Product product3 = new Product { Id = "1", Name = "Product1", Description = "Test Desc1", Price = 20, ProductType = ProductType.Bags };
            
            var lineItem2 = new LineItem
            {
                Product = product2,
                Quantity = 1
            };
            var lineItem3 = new LineItem
            {
                Product = product3,
                Quantity = 1
            };
            return new Cart
            {
                Id = cartId,
                DateCreated = DateTime.Now,
                LineItems = new List<LineItem> { lineItem2, lineItem3 }
            };
        }

        private IEnumerable<Discount> GetExpectedDiscounts()
        {
            return new List<Discount>
            {
                new BuyXGetYDiscountOffEachExceptFirst(
                "Buy 2 or more Bags of Pogs and get 50% off each bag (excluding the first one).",
                new List<ProductType> { ProductType.Bags }, 0.5m, 2),
                new BuyXGetYForFree(
                "Buy a Large bowl of Trifle and get a free Paper Mask.",
                new List<ProductType> { ProductType.LargeBowl }, 1, ProductType.PaperMask, 1),
                new BuyXGetYDiscountOffWhole(
                "Buy 100 or more Shurikens and get 30% off whole basket.",
                new List<ProductType> { ProductType.Shurikens }, 0.3m, 100)
            };
        }

        private Cart GetExpectedValidCart(Guid cartId)
        {
            Product product1 = new Product { Id = "1", Name = "Product1", Description = "Test Desc1", Price = 1, ProductType = ProductType.LargeBowl };
            Product product2 = new Product { Id = "2", Name = "Product2", Description = "Test Desc2", Price = 4, ProductType = ProductType.Shurikens };
            Product product3 = new Product { Id = "1", Name = "Product1", Description = "Test Desc1", Price = 20, ProductType = ProductType.Bags };
            var lineItem1 = new LineItem
            {
                Product = product1,
                Quantity = 1
            };
            var lineItem2 = new LineItem
            {
                Product = product2,
                Quantity = 100
            };
            var lineItem3 = new LineItem
            {
                Product = product3,
                Quantity = 2
            };
            return new Cart
            {
                Id = cartId,
                DateCreated = DateTime.Now,
                LineItems = new List<LineItem> { lineItem1, lineItem2, lineItem3}
            };
        }
    }
}
