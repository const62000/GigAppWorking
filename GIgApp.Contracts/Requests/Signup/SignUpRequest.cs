using System;
using System.Collections.Generic;
using GIgApp.Contracts.Enums;

namespace GIgApp.Contracts.Requests.Signup
{
    public class SignupRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<UserType> JobType { get; set; }
        public AddressRequest Address { get; set; }
        public List<LicenseRequest> License { get; set; }

        public SignupRequest(string firstName, string lastName, string email, string password, List<UserType> jobType, AddressRequest address, List<LicenseRequest> license)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            JobType = jobType;
            Address = address;
            License = license;
        }
    }

    public class AddressRequest
    {
        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string PostalCode { get; set; }


        public AddressRequest(string streetAddress1, string streetAddress2, string city, string country, decimal latitude, decimal longitude, string postalCode)
        {
            StreetAddress1 = streetAddress1;
            StreetAddress2 = streetAddress2;
            City = city;
            Country = country;
            Latitude = latitude;
            Longitude = longitude;
            PostalCode = postalCode;
        }
    }

    public class LicenseRequest
    {
        public string LicenseName { get; set; } = string.Empty;
        public string LicensesNumber { get; set; } = string.Empty;
        public DateTime DateOfIssue { get; set; }
        public string FileUrl { get; set; } = string.Empty;
        public string IssuedBy { get; set; } = string.Empty;

        public LicenseRequest(string licenseName, string licensesNumber, DateTime dateOfIssue, string fileUrl, string issuedBy)
        {
            LicenseName = licenseName;
            LicensesNumber = licensesNumber;
            DateOfIssue = dateOfIssue;
            FileUrl = fileUrl;
            IssuedBy = issuedBy;
        }
    }
}
