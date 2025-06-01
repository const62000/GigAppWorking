
namespace VMS.Client.ViewModels
{
    public class GoogleAddress
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string PostalCode { get; set; }
    }
}