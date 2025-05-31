using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Responses.Jobs
{
    public class JobQuestionnaireAnswerResult
    {
        public int Id { get; set; }
        public long JobApplicationId { get; set; }
        public long QuestionId { get; set; }
        public long UserId { get; set; }
        public string Answer { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public QuestionData QuestionData { get; set; } = new();
        public UserData UserData { get; set; } = new();
        //Need  review
        public List<FreelancerLicense> FreelancerLicenseData { get; set; } = new();
    }
    public class QuestionData
    {
        public long Id { get; set; }
        public long JobId { get; set; }
        public string Title { get; set; } = null!;
    }

    public class UserData
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
    public class FreelancerLicense
    {
        public int Id { get; set; }

        public long? FreelancerUserId { get; set; }

        public string? LicenseName { get; set; }

        public string? LicenseNumber { get; set; }

        public string IssuedBy { get; set; } = null!;

        public DateOnly IssuedDate { get; set; }

        public string LicenseStatus { get; set; } = "Not Approved";

        public string? RejectionReason { get; set; }

        public string? LicenseFileType { get; set; }

        public string? LicenseFileUrl { get; set; }
    }
}
