using Domain.Discounts;
using Domain.Entities;
using Domain.Repository;
using Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class DiscountServiceTests
    {
        private Dictionary<ProductType, List<Func<LineItem, bool>>> GetCompiledRuleDictionary()
        {
            List<Rule> bagsRules = new List<Rule>
            {
                 new Rule ( "Name", ExpressionType.Equal, "Bags"),
                 new Rule ( "Quantity", ExpressionType.GreaterThanOrEqual, "2")
            };
            List<Rule> largeBowlRules = new List<Rule>
            {
                 new Rule ( "Name", ExpressionType.Equal, "LargeBowl"),
                 new Rule ( "Quantity", ExpressionType.GreaterThanOrEqual, "1")
            };
            List<Rule> shurikensRules = new List<Rule>
            {
                 new Rule ( "Name", ExpressionType.Equal, "Shurikens"),
                 new Rule ( "Quantity", ExpressionType.GreaterThanOrEqual, "100")
            };

            var compiledBagsRulesrRules = PrecompiledRules.CompileRule(new List<LineItem>(), bagsRules);
            var compiledLargeBowlRulesRules = PrecompiledRules.CompileRule(new List<LineItem>(), largeBowlRules);
            var compiledShurikensRulesRules = PrecompiledRules.CompileRule(new List<LineItem>(), shurikensRules);

            var compiledRulesDictionary = new Dictionary<ProductType, List<Func<LineItem, bool>>>();

            compiledRulesDictionary.Add(ProductType.Bags, compiledBagsRulesrRules);
            compiledRulesDictionary.Add(ProductType.LargeBowl, compiledLargeBowlRulesRules);
            compiledRulesDictionary.Add(ProductType.Shurikens, compiledShurikensRulesRules);
            return compiledRulesDictionary;
        }

        [TestMethod]
        public async Task Should_Return_Discount_For_Valid_Bags()
        {
            //Arrange
            var expected = GetCompiledRuleDictionary();
            Product product1 = new Product { Id = "1", Name = "Product1", Description = "Test Desc1", Price = 2, ProductType = ProductType.Bags };
            var lineItem = new LineItem
            {
                Product = product1,
                Quantity = 2
            };
            
            var lineItems = new List<LineItem>{ lineItem };
            var mockDiscountFactory = new Mock<IDiscountFactory>();
            var mockRuleService = new Mock<IRuleService>();
            mockRuleService.Setup(x => x.GetCompiledRuleDictionary()).Returns(expected);
            mockDiscountFactory.Setup(x => x.GetDiscount(It.IsAny<ProductType>())).Returns(
                new BuyXGetYDiscountOffEachExceptFirst(
                "Buy 2 or more Bags of Pogs and get 50% off each bag (excluding the first one).",
                new List<ProductType> { ProductType.Bags }, 0.5m, 2));

            var discountService = new DiscountService(mockRuleService.Object, mockDiscountFactory.Object);

            // Act
            var results = await discountService.GetDiscounts(lineItems);

            //Assert
            Assert.AreEqual(results.Count(), 1);
        }

        [TestMethod]
        public async Task Should_Not_Return_Discount_For_Invalid_Bags()
        {
            //Arrange
            var expected = GetCompiledRuleDictionary();
            Product product = new Product { Id = "1", Name = "Product1", Description = "Test Desc1", Price = 2, ProductType = ProductType.Bags };
            var lineItem = new LineItem
            {
                Product = product,
                Quantity = 1
            };

            var lineItems = new List<LineItem> { lineItem };
            var mockDiscountFactory = new Mock<IDiscountFactory>();
            var mockRuleService = new Mock<IRuleService>();
            mockRuleService.Setup(x => x.GetCompiledRuleDictionary()).Returns(expected);
            mockDiscountFactory.Setup(x => x.GetDiscount(It.IsAny<ProductType>())).Returns(
                new BuyXGetYDiscountOffEachExceptFirst(
                "Buy 2 or more Bags of Pogs and get 50% off each bag (excluding the first one).",
                new List<ProductType> { ProductType.Bags }, 0.5m, 2));

            var discountService = new DiscountService(mockRuleService.Object, mockDiscountFactory.Object);

            // Act
            var results = await discountService.GetDiscounts(lineItems);

            //Assert
            Assert.AreEqual(results.Count(), 0);
        }

        [TestMethod]
        public async Task Should_Return_Discount_For__Valid_Shurikens()
        {
            //Arrange
            var expected = GetCompiledRuleDictionary();
            Product product = new Product { Id = "1", Name = "Product1", Description = "Test Desc1", Price = 2, ProductType = ProductType.Shurikens };
            var lineItem = new LineItem
            {
                Product = product,
                Quantity = 100
            };

            var lineItems = new List<LineItem> { lineItem };
            var mockDiscountFactory = new Mock<IDiscountFactory>();
            var mockRuleService = new Mock<IRuleService>();
            mockRuleService.Setup(x => x.GetCompiledRuleDictionary()).Returns(expected);
            mockDiscountFactory.Setup(x => x.GetDiscount(It.IsAny<ProductType>())).Returns(
                new BuyXGetYDiscountOffWhole(
                "Buy 100 or more Shurikens and get 30% off whole basket.",
                new List<ProductType> { ProductType.Shurikens }, 0.3m, 100));

            var discountService = new DiscountService(mockRuleService.Object, mockDiscountFactory.Object);

            // Act
            var results = await discountService.GetDiscounts(lineItems);

            //Assert
            Assert.AreEqual(results.Count(), 1);
        }

        [TestMethod]
        public async Task Should_Not_Return_Discount_For__Invalid_Shurikens()
        {
            //Arrange
            var expected = GetCompiledRuleDictionary();
            Product product = new Product { Id = "1", Name = "Product1", Description = "Test Desc1", Price = 2, ProductType = ProductType.Shurikens };
            var lineItem = new LineItem
            {
                Product = product,
                Quantity = 99
            };

            var lineItems = new List<LineItem> { lineItem };
            var mockDiscountFactory = new Mock<IDiscountFactory>();
            var mockRuleService = new Mock<IRuleService>();
            mockRuleService.Setup(x => x.GetCompiledRuleDictionary()).Returns(expected);
            mockDiscountFactory.Setup(x => x.GetDiscount(It.IsAny<ProductType>())).Returns(
                new BuyXGetYDiscountOffWhole(
                "Buy 100 or more Shurikens and get 30% off whole basket.",
                new List<ProductType> { ProductType.Shurikens }, 0.3m, 100));

            var discountService = new DiscountService(mockRuleService.Object, mockDiscountFactory.Object);

            // Act
            var results = await discountService.GetDiscounts(lineItems);

            //Assert
            Assert.AreEqual(results.Count(), 0);
        }

        [TestMethod]
        public async Task Should_Return_Discount_For_Valid_LargeBowls()
        {
            //Arrange
            var expected = GetCompiledRuleDictionary();
            Product product1 = new Product { Id = "1", Name = "Product1", Description = "Test Desc1", Price = 1, ProductType = ProductType.LargeBowl };
            //Product product2 = new Product { Id = "2", Name = "Product2", Description = "Test Desc2", Price = 4, ProductType = ProductType.PaperMask };
            var lineItem = new LineItem
            {
                Product = product1,
                Quantity = 1
            };

            var lineItems = new List<LineItem> { lineItem };
            var mockDiscountFactory = new Mock<IDiscountFactory>();
            var mockRuleService = new Mock<IRuleService>();
            mockRuleService.Setup(x => x.GetCompiledRuleDictionary()).Returns(expected);
            mockDiscountFactory.Setup(x => x.GetDiscount(It.IsAny<ProductType>())).Returns(
                new BuyXGetYForFree(
                "Buy a Large bowl of Trifle and get a free Paper Mask.",
                new List<ProductType> { ProductType.LargeBowl }, 1, ProductType.PaperMask, 1));

            var discountService = new DiscountService(mockRuleService.Object, mockDiscountFactory.Object);

            // Act
            var results = await discountService.GetDiscounts(lineItems);

            //Assert
            Assert.AreEqual(results.Count(), 1);
        }

        [TestMethod]
        public async Task Should_Not_Return_Discount_For_Invalid_LargeBowl()
        {
            //Arrange
            var expected = GetCompiledRuleDictionary();
            Product product = new Product { Id = "1", Name = "Product1", Description = "Test Desc1", Price = 2, ProductType = ProductType.Bags };
            var lineItem = new LineItem
            {
                Product = product,
                Quantity = 0
            };

            var lineItems = new List<LineItem> { lineItem };
            var mockDiscountFactory = new Mock<IDiscountFactory>();
            var mockRuleService = new Mock<IRuleService>();
            mockRuleService.Setup(x => x.GetCompiledRuleDictionary()).Returns(expected);
            mockDiscountFactory.Setup(x => x.GetDiscount(It.IsAny<ProductType>())).Returns(
                new BuyXGetYForFree(
                "Buy a Large bowl of Trifle and get a free Paper Mask.",
                new List<ProductType> { ProductType.LargeBowl }, 1, ProductType.PaperMask, 1));

            var discountService = new DiscountService(mockRuleService.Object, mockDiscountFactory.Object);

            // Act
            var results = await discountService.GetDiscounts(lineItems);

            //Assert
            Assert.AreEqual(results.Count(), 0);
        }

        [TestMethod]
        public async Task Should_Return_Discount_For_Valid_Items()
        {
            //Arrange
            var expected = GetCompiledRuleDictionary();
            Product product1 = new Product { Id = "1", Name = "Product1", Description = "Test Desc1", Price = 1, ProductType = ProductType.LargeBowl };
            Product product2 = new Product { Id = "2", Name = "Product2", Description = "Test Desc2", Price = 4, ProductType = ProductType.Shurikens };
            Product product3 = new Product { Id = "1", Name = "Product1", Description = "Test Desc1", Price = 2, ProductType = ProductType.Bags };
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
            var discounts = new Dictionary<ProductType, Discount>();
            discounts.Add(ProductType.Bags, new BuyXGetYDiscountOffEachExceptFirst(
                "Buy 2 or more Bags of Pogs and get 50% off each bag (excluding the first one).",
                new List<ProductType> { ProductType.Bags }, 0.5m, 2));
            discounts.Add(ProductType.LargeBowl, new BuyXGetYForFree(
                "Buy a Large bowl of Trifle and get a free Paper Mask.",
                new List<ProductType> { ProductType.LargeBowl }, 1, ProductType.PaperMask, 1));
            discounts.Add(ProductType.Shurikens, new BuyXGetYDiscountOffWhole(
                "Buy 100 or more Shurikens and get 30% off whole basket.",
                new List<ProductType> { ProductType.Shurikens }, 0.3m, 100));

            var lineItems = new List<LineItem> { lineItem1, lineItem2, lineItem3 };
            var mockDiscountFactory = new Mock<IDiscountFactory>();
            var mockRuleService = new Mock<IRuleService>();
            mockRuleService.Setup(x => x.GetCompiledRuleDictionary()).Returns(expected);
            mockDiscountFactory.Setup(x => x.GetDiscount(It.IsAny<ProductType>()))
                .Returns(discounts[It.IsAny<ProductType>()]);

            var discountService = new DiscountService(mockRuleService.Object, mockDiscountFactory.Object);

            // Act
            var results = await discountService.GetDiscounts(lineItems);

            //Assert
            Assert.AreEqual(results.Count(), 3);
        }

        [TestMethod]
        public async Task Should_Not_Return_Discount_For_Invalid_Items()
        {
            //Arrange
            var expected = GetCompiledRuleDictionary();
            Product product2 = new Product { Id = "2", Name = "Product2", Description = "Test Desc2", Price = 4, ProductType = ProductType.Shurikens };
            Product product3 = new Product { Id = "1", Name = "Product1", Description = "Test Desc1", Price = 2, ProductType = ProductType.Bags };
            var lineItem2 = new LineItem
            {
                Product = product2,
                Quantity = 99
            };
            var lineItem3 = new LineItem
            {
                Product = product3,
                Quantity = 1
            };
            var discounts = new Dictionary<ProductType, Discount>();
            discounts.Add(ProductType.Bags, new BuyXGetYDiscountOffEachExceptFirst(
                "Buy 2 or more Bags of Pogs and get 50% off each bag (excluding the first one).",
                new List<ProductType> { ProductType.Bags }, 0.5m, 2));
            discounts.Add(ProductType.LargeBowl, new BuyXGetYForFree(
                "Buy a Large bowl of Trifle and get a free Paper Mask.",
                new List<ProductType> { ProductType.LargeBowl }, 1, ProductType.PaperMask, 1));
            discounts.Add(ProductType.Shurikens, new BuyXGetYDiscountOffWhole(
                "Buy 100 or more Shurikens and get 30% off whole basket.",
                new List<ProductType> { ProductType.Shurikens }, 0.3m, 100));

            var lineItems = new List<LineItem> { lineItem2, lineItem3 };
            var mockDiscountFactory = new Mock<IDiscountFactory>();
            var mockRuleService = new Mock<IRuleService>();
            mockRuleService.Setup(x => x.GetCompiledRuleDictionary()).Returns(expected);
            mockDiscountFactory.Setup(x => x.GetDiscount(It.IsAny<ProductType>()))
                .Returns(discounts[It.IsAny<ProductType>()]);

            var discountService = new DiscountService(mockRuleService.Object, mockDiscountFactory.Object);

            // Act
            var results = await discountService.GetDiscounts(lineItems);

            //Assert
            Assert.AreEqual(results.Count(), 0);
        }
    }
}
