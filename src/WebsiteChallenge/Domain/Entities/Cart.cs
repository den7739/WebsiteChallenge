using Domain.Discounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class Cart
    {
        private IList<LineItem> _LineItems = new List<LineItem>();
        private IList<Discount> _Discounts = new List<Discount>();

        public Guid Id { get; set; }
        public IList<LineItem> LineItems { get; set; }
            
        public decimal GrossTotal
        {
            get
            {
                return LineItems
                    .Sum(x => x.Product.Price * x.Quantity);
            }
        }

        public void AddDiscount(IEnumerable<Discount> discounts)
        {
            foreach (var discount in discounts)
            {
                discount.Cart = this;
                discount.ApplyDiscount();
                _Discounts.Add(discount);
            }
        }

        public DateTime DateCreated { get; set; }

    }
}
