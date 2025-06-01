using GIgApp.Contracts.Requests.Signup;
using GIgApp.Contracts.Requests.Vendor;

namespace VMS.Client.ViewModels;
public class RegistrationViewModel
{
    public SignupRequest SignupRequest { get; set; }
    public VendorRequest VendorRequest { get; set; }

    public RegistrationViewModel(SignupRequest signupRequest, VendorRequest vendorRequest)
    {
        SignupRequest = signupRequest;
        VendorRequest = vendorRequest;
    }
}
