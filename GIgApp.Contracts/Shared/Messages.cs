using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIgApp.Contracts.Shared
{
    public static class Messages
    {
        public static string UserAlreadyExists = "User already exists. Please try with different email id";
        public static string Login_Fail = "Wrong email or password.";
        public static string SignUp_Fail = "We couldn’t create your account. Please check your information and try again.";
        public static string Vendor_Created = "Vendor created successfully";
        public static string Vendor_Updated = "Vendor updated successfully";
        public static string Vendor_Deleted = "Vendor deleted successfully";
        public static string Vendor_Deleted_Fail = "Vendor can not deleted";
        public static string Vendor_Not_Found = "Vendor not found";
        public static string Vendor_Manager_Added = "Vendor manager added successfully";
        public static string Staff_Added = "Staff added successfully";
        public static string Staff_Not_Found = "Staff not found";
        public static string Error = "Error happend";
        public static string File_Not_Uploaded = "Please add file";
        public static string Logout = "Thank you for visiting! You have successfully logged out. See you next time!";
        public static string Logout_Fail = "Logout failed. Please try again.";
        public static string List_Empty = "Items list must contain more than one item.";
        public static string Facility_Created = "Your facility created successfully.";
        public static string Facility_Deleted = "Facilties deleted successfully";
        public static string Facility_Deleted_Fail = "Facilties can not deleted";
        public static string Hiring_Created = "Done Successfully";

        public static string Not_Freelancer = "You are not a freelancer";

        public static string BankAccount_Created = "Your Bank account created successfully but not verified";
        public static string BankAccount_Created_Fail = "Your Bank account can not created";
        public static string BankAccount_Verified = "Your Bank account verified successfully";
        public static string BankAccount_Verified_Fail = "Your Bank account can not verified";
        public static string BankAccount_Deleted = "Your Bank account deleted successfully";
        public static string BankAccount_Deleted_Fail = "Your Bank account can not deleted";
        public static string BankAccount_Updated = "Your Bank account updated successfully";
        public static string BankAccount_Updated_Fail = "Your Bank account can not updated";
        public static string BankAccount_Not_Found = "Your Bank account not found";

        public static string Add_Answer = "Your answer added successfully";

        public static string Job_Completed = "Job completed successfully";
        public static string Job_Completed_Fail = "Job can not completed";
        public static string Job_NotFound = "Job not found";
        public static string JobQuestions_Deleted = "Questions deleted successfully";
        public static string JobQuestions_Deleted_Fail = "Questions can not deleted";

        public static string JobApplication_NotFount = "This Job Application Not Found";
        public static string You_Are_The_Job_Manager = "You are the job manager";
        public static string You_Are_Occupied_With_Another_Job = "You are occupied with another job";
        public static string You_Are_Eligible_To_Apply = "You are eligible to apply";
        public static string JobApplication_AlreadyApplied = "You already applied for this job";
        public static string Address_Created = "Your address created successfully";
        public static string Freelancer_Already_Hired = "Freelancer is already hired for the same job";

        public static string Hiring_Status = "Hiring status update successfully";
        public static string Hiring_Fail_Status = "Hiring can not status updated";
        public static string TimeSheet_ClockIn_Fail = "You already clocked in";
        public static string TimeSheet_ClockIn = "You clocked in successfully";
        public static string TimeSheet_ClockOut_Fail = "You already clocked out";
        public static string TimeSheet_ClockOut = "You clocked out in successfully";

        public static string PaymentMethod_Created = "Your payment method added successfully";
        public static string PaymentMethod_Deleted = "Your payment method deleted successfully";
        public static string PaymentMethod_Deleted_Fail = "Your payment method can not deleted";
        public static string Dispute_Added = "You raises a dispute";
        public static string Dispute_Added_Fail = "You can't raise a dispute";
        public static string Dispute_Process_Added = "Your dispute process create successfully";


        //Genirces
        public static string User_Not_Found = "User not found";

        //stripe
        public static string Payment_Success = "Payment successful";
        public static string Payment_Failed = "Payment failed";
        public static string Payment_Method_Added = "Payment method added successfully";
        public static string PaymentMethod_Created_Fail = "Payment method can not created";
        public static string Payment_Paid = "Payment paid successfully";
        public static string PaymentMethod_Not_Found = "Payment method not found";

        //TimeSheet
        public static string TimeSheet_Location_Added = "Time sheet location added successfully";
        public static string TimeSheet_Status_Changed = "Time sheet status changed successfully";
        public static string TimeSheet_Approval_Status_Changed = "Time sheet approval status changed successfully";

        //Stripe Connect
        public static string StripeAccountIdUpdated = "Stripe account id updated successfully";
        public static string StripeAccountIdUpdateFailed = "Stripe account id can not updated";
    }
}
