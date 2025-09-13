
namespace CebuCrust_api.ServiceModels
{
    public class OrderRequest
    {
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public string? OrderInstruction { get; set; }
        public string? OrderStatus { get; set; }
        public string? OrderEstimate { get; set; }
        public List<OrderItemRequest> Items { get; set; } = new();
    }
}
