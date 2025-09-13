namespace CebuCrust_api.ServiceModels
{
    public class CartRequest
    {
        public int UserId { get; set; }
        public int PizzaId { get; set; }
        public int Quantity { get; set; }
    }
}
