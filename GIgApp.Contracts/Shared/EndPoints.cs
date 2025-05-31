using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Shared
{
    public static class EndPoints
    {
        //Auth0 EndPoints
        public static string Auth0_Login_EndPoint = "oauth/token";
        public static string Auth0_signup_EndPoint = "dbconnections/signup";

        //For Auth
        public static string Login = "api/login";
        public static string Role = "api/role";
        public static string SignUp = "api/signup";
        public static string Logout = "api/logout";
        public static string VendorLogin = "api/vendor-login";
        public static string RegisterAdmin = "api/register-admin";

        //For Upload File
        public static string Upload_File_EndPoint = "api/addFile";

        //For Vendor
        public static string Vendor_EndPoint = "api/vendor";
        public static string Staff_EndPoint = "api/Staff";
        public static string VendorManager_EndPoint = "api/vendor-manager";

        //For Facilities
        public static string Facilities_EndPoint = "api/facilities";
        public static string Facility_Invitation = "/api/facilities/invite";
        public static string Facilities_VMS = "/api/facilities/vms";
        public static string Client_Location = "/api/client-location";

        //For Jobs
        public static string CreateJob = "api/createjob";
        public static string JobDetails = "api/jobdetails/{jobId}";
        public static string JobList = "api/jobList";
        public static string UpdateJob = "api/updatejob";
        public static string CompleteJob = "api/completejob/{jobId}";
        public static string DeleteJob = "api/deletejob/{jobId}";
        public static string JobsByStatus = "api/jobs/status/{status}";
        public static string DeleteJobs = "api/deletejobs";
        public static string JobWithdrawal = "api/jobWithdrawal";
        public static string AssignJobManager = "api/jobs/{jobId}/assign-manager";

        //For Job Applications
        public static string CreateJobApplication = "/api/jobs/{jobId}/applications";
        public static string GetJobApplicationById = "/api/jobs/{jobId}/applications";
        public static string CheckApply = CreateJobApplication + "/applyCheck";
        public static string View_Job_Application = "/api/jobs/application/{id}";

        //For User endpoints
        public static string CurrentUser = "api/currentUser";
        public static string Update_User_Profile = "/api/updateCurrentUser";
        public static string Get_USer_Notifications = "/api/notifications/{page}/{pageSize}";
        public static string Users = "/api/users";
        //For Job Questions
        public static string CreateJobQuestion = "api/job-questions/create";
        public static string JobQuestionByJobId = "api/job-questions/by-job/{jobid}";
        public static string JobQuestionnaireAnswersByJobApplicationId = "api/job-questionnaire-answers{jobApplicationId}";
        public static string Answer_EndPoint = "api/job-questionnaire-answers/create";
        public static string Delete_Job_Questions = "/api/job-questions/delete-multiple";
        public static string Delete_Job_Question = "/api/job-questions/delete/{id}";

        //For Hiring
        public static string Hiring = "api/hiring";
        public static string Hiring_Job = "api/hiring/job";
        public static string Hiring_Freelancer = "api/hiring/freelancer";
        public static string Hiring_Manager = "api/hiring/manager";
        public static string Hiring_Status = "api/hiring-status";

        //For BankAccount
        public static string BackAccount = "api/bank-accounts";
        public static string VerifyBankAccount = "api/verify-bank-account";

        //For Countries
        public static string Counties = "api/countries";
        public static string Cities = "api/cities/{country}";

        //For Address
        public static string GetAddress = "api/address";

        //License Info
        public static string GetFreelancerLicenseInfo = "api/freelancer-licenseInfo/{freelancerUserId}";

        //For Freelancers
        public static string Freelancers = "api/freelancers";

        //TimeSheet
        public static string ClockIn = "api/clockin";
        public static string ClockOut = "api/clockOut";
        public static string TimeSheet = "api/time-sheet";
        public static string TimeSheet_User = TimeSheet + "/user";
        public static string TimeSheet_Hired = TimeSheet + "/hired";
        public static string TimeSheetLocation = "api/time-sheet-location";
        public static string ChangeTimeSheetStatus = "api/time-sheet-status";
        public static string ChangeTimeSheetApprovalStatus = "api/time-sheet-approval-status";


        //PaymentMethods
        public static string Payment = "api/payment";
        public static string PaymentMethod = "api/payment-method";
        public static string CheckPaymentMethod = "api/payment-method/check";
        public static string Dispute = "api/dispute";
        public static string ProcessDispute = "api/process-dispute";

        //For Stripe Connect
        public static string AddStripeConnect = "api/stripe-connect";
        public static string StripeWebHook = "api/s tripe-webhook";

        //For Rating
        public static string Rating = "api/rating";
        public static string CaregiverRating = "api/caregivers/ratings";
        public static string CaregiverRatingByJobId = "api/caregivers/{caregiverId}/ratings/{jobId}";
        public static string FacilityRating = "api/facilities/ratings";
    }
}
