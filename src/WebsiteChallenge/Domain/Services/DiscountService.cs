using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Discounts;
using Domain.Entities;
using Domain.ViewModels;

namespace Domain.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IRuleService ruleService;
        private readonly IDiscountFactory discountFactory;

        public DiscountService(IRuleService ruleService, IDiscountFactory discountFactory)
        {
            this.ruleService = ruleService;
            this.discountFactory = discountFactory;
        }

        public async Task<IEnumerable<Discount>> GetDiscounts(IEnumerable<LineItem> lineItems)
        {
            var discounts = new List<Discount>();
            Dictionary<ProductType, List<Func<LineItem, bool>>> compiledRulesDictionary = ruleService.GetCompiledRuleDictionary();
            foreach (var lineItem in lineItems)
            {
                foreach (var compiledRule in compiledRulesDictionary)
                {
                    if (compiledRule.Value.TakeWhile(rule => rule(lineItem)).Count() == compiledRule.Value.Count)
                    {
                        discounts.Add(discountFactory.GetDiscount(compiledRule.Key));
                    }
                }
            }
            return await Task.FromResult(discounts);
        }
    }
}
