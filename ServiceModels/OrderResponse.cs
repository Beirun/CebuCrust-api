
namespace CebuCrust_api.ServiceModels
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public int LocationId { get; set; }
        public string? OrderInstruction { get; set; }
        public string? OrderStatus { get; set; }
        public string? OrderEstimate { get; set; }
        public List<OrderItemResponse> Items { get; set; } = new();
    }
}
