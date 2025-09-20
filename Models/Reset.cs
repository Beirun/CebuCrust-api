namespace CebuCrust_api.Models
{
    public class Reset
    {
        public int ResetId {  get; set; }
        public int UserId { get; set; }
        public string ResetCode { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
    }
}
