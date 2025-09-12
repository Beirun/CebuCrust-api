using CebuCrust_api.Models;

namespace CebuCrust_api.Models
{
    public class OrderList
    {
        public int OrderId { get; set; }
        public int PizzaId { get; set; }
        public int Quantity { get; set; }

        public Order Order { get; set; } = null!;
        public Pizza Pizza { get; set; } = null!;
    }
}
