using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ViewModels
{
    public class CartViewModel
    {
        public decimal CartSubTotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal Total { get; set; }
        public IEnumerable<LineItemViewModel> LineItems { get; set; }
        public IEnumerable<DiscountViewModel> Discounts { get; set; }
    }
}
