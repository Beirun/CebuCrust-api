namespace CebuCrust_api.ServiceModels
{
    public class PizzaResponse
    {
        public int PizzaId { get; set; }
        public string PizzaName { get; set; } = "";
        public string PizzaDescription { get; set; } = "";
        public string PizzaCategory { get; set; } = "";
        public decimal PizzaPrice { get; set; }
        public Boolean IsAvailable { get; set; }
        public byte[]? pizzaImage { get; set; }
    }
}
