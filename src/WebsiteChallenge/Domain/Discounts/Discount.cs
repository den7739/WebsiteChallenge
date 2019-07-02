using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Discounts
{
    public abstract class Discount
    {
        public Discount(string name)
        {
            Name = name;
        }

        public abstract Cart ApplyDiscount();
        public virtual Cart Cart { get; set; }
        public virtual string Name { get; set; }
    }
}
