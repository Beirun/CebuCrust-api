namespace CebuCrust_api.ServiceModels
{
    public class LocationResponse
    {
        public int LocationId { get; set; }
        public string? LocationCity { get; set; }
        public string? LocationBrgy { get; set; }
        public string? LocationStreet { get; set; }
        public string? LocationHouseNo { get; set; }

    }
}
