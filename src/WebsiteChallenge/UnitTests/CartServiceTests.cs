using Domain.Entities;
using Domain.Repository;
using Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class CartServiceTests
    {
        [TestMethod]
        public void Should_Add_New_Lines()
        {
            //Arrange
            Product product1 = new Product { Id = "1", Name = "Product1", Description = "Test Desc1", Price = 1, ProductType = ProductType.Bags };
            Product product2 = new Product { Id = "2", Name = "Product2", Description = "Test Desc2", Price = 2, ProductType = ProductType.PaperMask };
            var cartId = Guid.NewGuid();
            var expected = new Cart
            {
                Id = cartId,
                DateCreated = DateTime.Now,
                LineItems = new List<LineItem>()
            };
            var mockCartRepository = new Mock<ICartRepository>();
            CartService cart = new CartService(mockCartRepository.Object);
            mockCartRepository.Setup(m => m.GetById(cartId))
                .Returns(expected);

            // Act
            cart.AddItem(cartId, new LineItem { Product = product1, Quantity = 1 });
            cart.AddItem(cartId, new LineItem { Product = product2, Quantity = 1 });
            var result = mockCartRepository.Object.GetById(cartId);

            //Assert
            mockCartRepository.Verify(t => t.AddOrUpdate(It.IsAny<Cart>()), Times.Exactly(2));
            Assert.AreEqual(result.LineItems.Count, 2);
            Assert.AreEqual(result.LineItems[0].Product, product1);
            Assert.AreEqual(result.LineItems[0].Quantity, 1);
            Assert.AreEqual(result.LineItems[0].Subtotal, 1);
            Assert.AreEqual(result.LineItems[1].Product, product2);
            Assert.AreEqual(result.LineItems[1].Quantity, 1);
            Assert.AreEqual(result.LineItems[1].Subtotal, 2);
            Assert.AreEqual(result.GrossTotal, 3);
        }

        [TestMethod]
        public void Should_Add_Quantity_For_Existing_Lines()
        {
            //Arrange
            Product product1 = new Product { Id = "1", Name = "Product1", Description = "Test Desc1", Price = 1, ProductType = ProductType.Bags };
            Product product2 = new Product { Id = "2", Name = "Product2", Description = "Test Desc2", Price = 2, ProductType = ProductType.PaperMask };
            var cartId = Guid.NewGuid();
            var expected = new Cart
            {
                Id = cartId,
                DateCreated = DateTime.Now,
                LineItems = new List<LineItem>
                {
                    new LineItem
                    {
                        Product = product1,
                        Quantity = 1
                    }
                }
            };
            var mockCartRepository = new Mock<ICartRepository>();
            CartService cart = new CartService(mockCartRepository.Object);
            mockCartRepository.Setup(m => m.GetById(cartId))
                .Returns(expected);

            // Act
            cart.AddItem(cartId, new LineItem { Product = product1, Quantity = 1 });
            cart.AddItem(cartId, new LineItem { Product = product2, Quantity = 1 });
            var result = mockCartRepository.Object.GetById(cartId);

            //Assert
            mockCartRepository.Verify(t => t.AddOrUpdate(It.IsAny<Cart>()), Times.Exactly(2));
            Assert.AreEqual(result.LineItems.Count, 2);
            Assert.AreEqual(result.LineItems[0].Product, product1);
            Assert.AreEqual(result.LineItems[0].Quantity, 2);
            Assert.AreEqual(result.LineItems[0].Subtotal, 2);
            Assert.AreEqual(result.LineItems[1].Product, product2);
            Assert.AreEqual(result.LineItems[1].Quantity, 1);
            Assert.AreEqual(result.LineItems[1].Subtotal, 2);
            Assert.AreEqual(result.GrossTotal, 4);
        }

        [TestMethod]
        public void Should_Remove_Whole_Line()
        {
            //Arrange
            Product product1 = new Product { Id = "1", Name = "Product1", Description = "Test Desc1", Price = 1, ProductType = ProductType.Bags };
            Product product2 = new Product { Id = "2", Name = "Product2", Description = "Test Desc2", Price = 2, ProductType = ProductType.PaperMask };
            var cartId = Guid.NewGuid();
            var lineItem = new LineItem
            {
                Product = product2,
                Quantity = 1
            };

            var expected = new Cart
            {
                Id = cartId,
                DateCreated = DateTime.Now,
                LineItems = new List<LineItem> {
                    new LineItem
                    {
                        Product = product1,
                        Quantity = 2
                    },
                    lineItem
                }
            };
            var mockCartRepository = new Mock<ICartRepository>();
            CartService cart = new CartService(mockCartRepository.Object);
            mockCartRepository.Setup(m => m.GetById(cartId))
                .Returns(expected);

            // Act
            cart.RemoveLine(cartId, lineItem, true);
            var result = mockCartRepository.Object.GetById(cartId);

            //Assert
            mockCartRepository.Verify(t => t.AddOrUpdate(It.IsAny<Cart>()), Times.Exactly(1));
            Assert.AreEqual(result.LineItems.Count, 1);
            Assert.AreEqual(result.LineItems[0].Product, product1);
            Assert.AreEqual(result.LineItems[0].Quantity, 2);
            Assert.AreEqual(result.LineItems[0].Subtotal, 2);
            Assert.AreEqual(result.GrossTotal, 2);
        }

        [TestMethod]
        public void Should_Remove_Line()
        {
            //Arrange
            Product product1 = new Product { Id = "1", Name = "Product1", Description = "Test Desc1", Price = 1, ProductType = ProductType.Bags };
            var cartId = Guid.NewGuid();
            var lineItem = new LineItem
            {
                Product = product1,
                Quantity = 2
            };

            var expected = new Cart
            {
                Id = cartId,
                DateCreated = DateTime.Now,
                LineItems = new List<LineItem> { lineItem }
            };
            var mockCartRepository = new Mock<ICartRepository>();
            CartService cart = new CartService(mockCartRepository.Object);
            mockCartRepository.Setup(m => m.GetById(cartId))
                .Returns(expected);

            // Act
            cart.RemoveLine(cartId, lineItem, false);
            var result = mockCartRepository.Object.GetById(cartId);

            //Assert
            mockCartRepository.Verify(t => t.AddOrUpdate(It.IsAny<Cart>()), Times.Exactly(1));
            Assert.AreEqual(result.LineItems.Count, 1);
            Assert.AreEqual(result.LineItems[0].Product, product1);
            Assert.AreEqual(result.LineItems[0].Quantity, 1);
            Assert.AreEqual(result.LineItems[0].Subtotal, 1);
            Assert.AreEqual(result.GrossTotal, 1);
        }

        [TestMethod]
        public void Should_Clear_Card()
        {
            //Arrange
            Product product1 = new Product { Id = "1", Name = "Product1", Description = "Test Desc1", Price = 1, ProductType = ProductType.Bags };
            var cartId = Guid.NewGuid();
            var lineItem = new LineItem
            {
                Product = product1,
                Quantity = 2
            };

            var expected = new Cart
            {
                Id = cartId,
                DateCreated = DateTime.Now,
                LineItems = new List<LineItem> { lineItem }
            };
            var mockCartRepository = new Mock<ICartRepository>();
            CartService cart = new CartService(mockCartRepository.Object);
            mockCartRepository.Setup(m => m.GetById(cartId))
                .Returns(expected);

            // Act
            cart.Clear(cartId);
            var result = mockCartRepository.Object.GetById(cartId);

            //Assert
            mockCartRepository.Verify(t => t.Clear(It.IsAny<Cart>()), Times.Exactly(1));
        }
    }
}
