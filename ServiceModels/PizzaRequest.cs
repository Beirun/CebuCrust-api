namespace CebuCrust_api.ServiceModels
{
    public class PizzaRequest
    {
        public string PizzaName { get; set; } = "";
        public string PizzaDescription { get; set; } = "";
        public string PizzaCategory { get; set; } = "";
        public decimal PizzaPrice { get; set; }
        public Boolean IsAvailable { get; set; } = true;
        public IFormFile? Image { get; set; }
    }
}
