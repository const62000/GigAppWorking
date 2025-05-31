using GIgApp.Contracts.Requests.Signup;
using System.ComponentModel.DataAnnotations;
namespace GIgApp.Contracts.Requests.Vendor
{
    public class VendorRequest
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? ServicesOffered { get; set; }
        public string? Certifications { get; set; }
        public string? Status { get; set; }
    }
}
