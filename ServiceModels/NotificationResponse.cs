namespace CebuCrust_api.ServiceModels
{
    public class NotificationResponse
    {
        public int NotificationId { get; set; }
        public string? NotificationMessage { get; set; }
        public string? NotificationTitle { get; set; }

        public string? NotificationStatus { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
