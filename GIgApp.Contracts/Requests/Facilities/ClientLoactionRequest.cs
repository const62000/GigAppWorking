using GIgApp.Contracts.Requests.Signup;
namespace GIgApp.Contracts.Requests.Facilities
{
    public class ClientLocationRequest
    {
        public string? LocationName { get; set; }
        public long ClientId { get; set; }
        public AddressRequest? Address { get; set; } = new AddressRequest(string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, string.Empty);
    }
}
