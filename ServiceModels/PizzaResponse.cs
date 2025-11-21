namespace CebuCrust_api.ServiceModels
{
    public class PizzaResponse
    {
        public int PizzaId { get; set; }
        public string PizzaName { get; set; } = "";
        public string PizzaDescription { get; set; } = "";
        public string PizzaCategory { get; set; } = "";
        public decimal PizzaPrice { get; set; }
        public int Stock {get; set;}
        public int FavoriteCount {get; set;}
        public bool IsDeleted { get; set; }
        public byte[]? pizzaImage { get; set; }
    }
}
