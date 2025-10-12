
namespace CebuCrust_api.ServiceModels
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public int LocationId { get; set; }
        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? OrderInstruction { get; set; }
        public string? OrderStatus { get; set; }
        public string? OrderEstimate { get; set; }
        public DateTime DateCreated { get; set; }
        public LocationResponse? Location { get; set; }
        public List<OrderItemResponse> OrderLists { get; set; } = new();
    }
}
