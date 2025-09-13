namespace CebuCrust_api.ServiceModels
{
    public class RatingRequest
    {
        public int UserId { get; set; }
        public int PizzaId { get; set; }
        public int RatingValue { get; set; }
        public string? RatingMessage { get; set; }
    }
}
