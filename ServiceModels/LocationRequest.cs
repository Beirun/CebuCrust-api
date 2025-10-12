namespace CebuCrust_api.ServiceModels
{
    public class LocationRequest
    {
        public string? LocationCity { get; set; }
        public string? LocationBrgy { get; set; }
        public string? LocationStreet { get; set; }
        public string? LocationHouseNo { get; set; }
        public string? LocationPostal { get; set; }
        public string? LocationLandmark { get; set; }
        public bool? IsDefault { get; set; }
    }
}
