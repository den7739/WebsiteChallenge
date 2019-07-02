using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Services
{
    public interface IRuleService
    {
        Dictionary<ProductType, List<Func<LineItem, bool>>> GetCompiledRuleDictionary();
    }
}