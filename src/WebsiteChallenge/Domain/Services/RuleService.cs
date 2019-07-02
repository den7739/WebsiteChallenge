using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Domain.Entities;

namespace Domain.Services
{
    public class RuleService : IRuleService
    {
        public Dictionary<ProductType, List<Func<LineItem, bool>>> GetCompiledRuleDictionary()
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
    }
}
