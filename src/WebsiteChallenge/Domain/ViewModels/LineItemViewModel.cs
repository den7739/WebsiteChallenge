using Domain.Entities;

namespace Domain.ViewModels
{
    public class LineItemViewModel
    {
        public string ProductName { get; set; }
        public ProductType ProductType { get; set; }
        public int Count { get; set; }
        public decimal SubTotal { get; set; }
    }
}