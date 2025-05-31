-- PostgreSQL Database Creation Script

-- Addresses Table
CREATE TABLE "Addresses" (
    "Id" BIGSERIAL PRIMARY KEY,
    "Label" VARCHAR(255),
    "StreetAddress1" VARCHAR(255) NOT NULL,
    "StreetAddress2" VARCHAR(255),
    "City" VARCHAR(200) NOT NULL,
    "StateProvince" VARCHAR(100),
    "PostalCode" VARCHAR(20),
    "Latitude" DECIMAL(10, 7),
    "Longitude" DECIMAL(10, 7),
    "Country" VARCHAR(100) NOT NULL,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "ClientId" BIGINT,
    "UserId" BIGINT
);

-- BankAccounts Table
CREATE TABLE "BankAccounts" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" BIGINT NOT NULL,
    "BankName" TEXT NOT NULL,
    "BankAccountType" TEXT NOT NULL,
    "BankAccountNumber" TEXT NOT NULL,
    "BankAccountName" TEXT NOT NULL,
    "BankSwiftCode" TEXT NOT NULL,
    "BankCountry" TEXT NOT NULL,
    "status" TEXT NOT NULL,
    "CreateAt" TIMESTAMP NOT NULL,
    "StripeBankAccountId" VARCHAR(255),
    "StripeCustomerId" VARCHAR(255)
);

-- Clients Table
CREATE TABLE "Clients" (
    "Id" BIGSERIAL PRIMARY KEY,
    "Name" VARCHAR(100) NOT NULL,
    "AdminUserId" BIGINT,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ClientLocations Table
CREATE TABLE "ClientLocations" (
    "Id" BIGSERIAL PRIMARY KEY,
    "Name" VARCHAR(100) NOT NULL,
    "AddressId" BIGINT,
    "LocationName" VARCHAR(20) NOT NULL,
    "ClientId" BIGINT,
    "CreatedByUserId" BIGINT,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ClientManagers Table
CREATE TABLE "ClientManagers" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" BIGINT,
    "ClientId" BIGINT,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- FacilityManagers Table
CREATE TABLE "FacilityManagers" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" BIGINT,
    "FacilityId" BIGINT,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- FreelancerLicenses Table
CREATE TABLE "FreelancerLicenses" (
    "Id" SERIAL PRIMARY KEY,
    "FreelancerUserId" BIGINT,
    "LicenseName" TEXT,
    "LicenseNumber" TEXT,
    "IssuedBy" VARCHAR(100) NOT NULL,
    "IssuedDate" DATE NOT NULL,
    "LicenseStatus" VARCHAR(20) NOT NULL,
    "RejectionReason" TEXT,
    "LicenseFileType" VARCHAR(20),
    "LicenseFileUrl" TEXT,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Freelancers Table
CREATE TABLE "Freelancers" (
    "UserId" INTEGER PRIMARY KEY,
    "AddressId" BIGINT,
    "LicenseInfo" TEXT,
    "BankAccountInfo" TEXT,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Invoices Table
CREATE TABLE "Invoices" (
    "Id" SERIAL PRIMARY KEY,
    "ContractId" INTEGER,
    "Amount" DECIMAL(10, 2) NOT NULL,
    "Status" VARCHAR(20) NOT NULL,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- JobApplications Table
CREATE TABLE "JobApplications" (
    "Id" BIGSERIAL PRIMARY KEY,
    "JobId" BIGINT,
    "FreelancerUserId" BIGINT,
    "JobApplicationStatus" VARCHAR(20) NOT NULL,
    "WithdrawalStatus" VARCHAR(20),
    "WithdrawalReason" TEXT,
    "WithdrawalDate" TIMESTAMP,
    "Proposal" TEXT NOT NULL,
    "ProposalHourlyRate" DECIMAL(10, 2),
    "Viewed" BOOLEAN DEFAULT FALSE,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- JobHires Table
CREATE TABLE "JobHires" (
    "Id" SERIAL PRIMARY KEY,
    "JobId" BIGINT NOT NULL,
    "FreelancerId" BIGINT,
    "HiredManagerId" BIGINT NOT NULL,
    "Status" TEXT NOT NULL,
    "Note" TEXT,
    "StartTime" TIMESTAMP NOT NULL,
    "EndTime" TIMESTAMP NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL
);

-- JobQuestionnaireAnswers Table
CREATE TABLE "JobQuestionnaireAnswers" (
    "Id" SERIAL PRIMARY KEY,
    "JobApplicationId" BIGINT,
    "QuestionId" BIGINT,
    "UserId" BIGINT,
    "Answer" TEXT NOT NULL,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- JobQuestionnaires Table
CREATE TABLE "JobQuestionnaires" (
    "Id" BIGSERIAL PRIMARY KEY,
    "JobId" BIGINT,
    "Question" TEXT NOT NULL,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Jobs Table
CREATE TABLE "Jobs" (
    "Id" BIGSERIAL PRIMARY KEY,
    "Title" VARCHAR(100) NOT NULL,
    "Description" TEXT NOT NULL,
    "Requirements" TEXT,
    "Date" DATE NOT NULL,
    "Time" TIME NOT NULL,
    "Rate" DECIMAL(10, 2) NOT NULL,
    "Status" VARCHAR(20) NOT NULL,
    "JobManagerUserId" BIGINT,
    "AddressId" BIGINT,
    "LocationId" BIGINT,
    "ClientId" BIGINT,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "LicenseRequirments" TEXT,
    "HoursPerWeek" INTEGER,
    "JobType" INTEGER,
    "PaymentIntentId" VARCHAR(255),
    "PaymentMethodId" INTEGER,
    "StartedDate" TIMESTAMP,
    "EndedDate" TIMESTAMP
);

-- Notifications Table
CREATE TABLE "Notifications" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" BIGINT NOT NULL,
    "Title" TEXT,
    "Description" TEXT,
    "Image" TEXT,
    "Type" TEXT,
    "NotificationTime" TIMESTAMP NOT NULL,
    "HasViewed" BOOLEAN NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL
);

-- PaymentMethods Table
CREATE TABLE "PaymentMethods" (
    "id" SERIAL PRIMARY KEY,
    "user_id" BIGINT NOT NULL,
    "client_id" BIGINT,
    "card_last_four" VARCHAR(4) NOT NULL,
    "card_brand" VARCHAR(50) NOT NULL,
    "exp_month" INTEGER NOT NULL,
    "exp_year" INTEGER NOT NULL,
    "cardholder_name" VARCHAR(255) NOT NULL,
    "stripe_payment_method_id" VARCHAR(255) NOT NULL UNIQUE,
    "stripe_card_id" VARCHAR(255) NOT NULL UNIQUE,
    "is_default" BOOLEAN DEFAULT FALSE,
    "status" VARCHAR(20) DEFAULT 'active',
    "created_at" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "updated_at" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "StripeCustomerId" VARCHAR(255)
);

-- Payments Table
CREATE TABLE "Payments" (
    "id" SERIAL PRIMARY KEY,
    "job_id" BIGINT,
    "payment_method_id" INTEGER,
    "amount" DECIMAL(10, 2) NOT NULL,
    "status" VARCHAR(20) NOT NULL,
    "payment_type" VARCHAR(20) NOT NULL,
    "escrow_status" VARCHAR(20),
    "stripe_payment_intent_id" VARCHAR(255) UNIQUE,
    "stripe_transfer_id" VARCHAR(255) UNIQUE,
    "description" TEXT,
    "failure_reason" TEXT,
    "paid_by_user_id" BIGINT,
    "paid_to_user_id" BIGINT,
    "created_at" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "updated_at" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- TimeSheet Table
CREATE TABLE "TimeSheet" (
    "Id" BIGSERIAL PRIMARY KEY,
    "HiringId" INTEGER NOT NULL,
    "UserId" BIGINT NOT NULL,
    "ClockIn" TIMESTAMP NOT NULL,
    "ClockOut" TIMESTAMP,
    "TimeSheetNotes" TEXT,
    "Status" VARCHAR(50) NOT NULL,
    "TimeSheetApprovalStatus" VARCHAR(50)
);

-- TimeSheetLocation Table
CREATE TABLE "TimeSheetLocation" (
    "Id" BIGSERIAL PRIMARY KEY,
    "TimeSheetId" BIGINT NOT NULL,
    "LocationCapturedDateTime" TIMESTAMP NOT NULL,
    "Latitude" FLOAT,
    "Longitude" FLOAT
);

-- UserDevices Table
CREATE TABLE "UserDevices" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" BIGINT NOT NULL,
    "FirebaseToken" TEXT NOT NULL,
    "DeviceInfo" TEXT NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL
);

-- Users Table
CREATE TABLE "Users" (
    "Id" BIGSERIAL PRIMARY KEY,
    "Auth0UserId" VARCHAR(255) UNIQUE,
    "Email" VARCHAR(255) NOT NULL UNIQUE,
    "FirstName" VARCHAR(100),
    "LastName" VARCHAR(100),
    "Disabled" BOOLEAN DEFAULT FALSE,
    "DisabledByUserId" BIGINT,
    "AddressId" BIGINT,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "Picture" TEXT,
    "StripeAccountId" VARCHAR(255),
    "StripeCustomerId" TEXT
);

-- UserVendors Table
CREATE TABLE "UserVendors" (
    "Id" BIGSERIAL,
    "UserId" BIGINT NOT NULL,
    "VendorId" INTEGER NOT NULL,
    PRIMARY KEY ("UserId", "VendorId")
);

-- Vendors Table
CREATE TABLE "Vendors" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" BIGINT,
    "Name" TEXT,
    "ServicesOffered" TEXT,
    "Certifications" TEXT,
    "Status" VARCHAR(20) NOT NULL,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- VendorsRegistrants Table
CREATE TABLE "VendorsRegistrants" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" BIGINT NOT NULL,
    "VendorId" INTEGER NOT NULL,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Ratings Table
CREATE TABLE "Ratings" (
    "Id" SERIAL PRIMARY KEY,
    "RatingGivenByUserId" BIGINT NOT NULL,
    "JobId" BIGINT,
    "JobHireId" INTEGER,
    "JobStarRating" INTEGER,
    "CaregiverId" BIGINT,
    "CaregiverStarRating" INTEGER,
    "FacilityId" BIGINT,
    "FacilityStarRating" INTEGER,
    "JobManagerId" BIGINT,
    "JobManagerStarRating" INTEGER,
    "VendorId" INTEGER,
    "VendorStarRating" INTEGER,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP NOT NULL
);

-- Foreign Key Constraints

-- Addresses Foreign Keys
ALTER TABLE "Addresses" ADD CONSTRAINT "FK_Addresses_Client" 
    FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id") ON DELETE CASCADE;
ALTER TABLE "Addresses" ADD CONSTRAINT "FK_Addresses_User" 
    FOREIGN KEY ("UserId") REFERENCES "Users" ("Id");

-- BankAccounts Foreign Keys
ALTER TABLE "BankAccounts" ADD CONSTRAINT "FK_BankAccounts_Users_UserId" 
    FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE;

-- Clients Foreign Keys
ALTER TABLE "Clients" ADD CONSTRAINT "FK_Clients_AdminUser" 
    FOREIGN KEY ("AdminUserId") REFERENCES "Users" ("Id");

-- ClientLocations Foreign Keys
ALTER TABLE "ClientLocations" ADD CONSTRAINT "FK_ClientLocations_Address" 
    FOREIGN KEY ("AddressId") REFERENCES "Addresses" ("Id");
ALTER TABLE "ClientLocations" ADD CONSTRAINT "FK_ClientLocations_Client" 
    FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id");
ALTER TABLE "ClientLocations" ADD CONSTRAINT "FK_ClientLocations_CreatedByUser" 
    FOREIGN KEY ("CreatedByUserId") REFERENCES "Users" ("Id");

-- ClientManagers Foreign Keys
ALTER TABLE "ClientManagers" ADD CONSTRAINT "FK_ClientManagers_Client" 
    FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id");
ALTER TABLE "ClientManagers" ADD CONSTRAINT "FK_ClientManagers_User" 
    FOREIGN KEY ("UserId") REFERENCES "Users" ("Id");

-- FacilityManagers Foreign Keys
ALTER TABLE "FacilityManagers" ADD CONSTRAINT "FK_FacilityManagers_Facility" 
    FOREIGN KEY ("FacilityId") REFERENCES "Clients" ("Id");
ALTER TABLE "FacilityManagers" ADD CONSTRAINT "FK_FacilityManagers_User" 
    FOREIGN KEY ("UserId") REFERENCES "Users" ("Id");

-- FreelancerLicenses Foreign Keys
ALTER TABLE "FreelancerLicenses" ADD CONSTRAINT "FK_FreelancerLicenses_User" 
    FOREIGN KEY ("FreelancerUserId") REFERENCES "Users" ("Id");

-- Freelancers Foreign Keys
ALTER TABLE "Freelancers" ADD CONSTRAINT "FK_Freelancers_Address" 
    FOREIGN KEY ("AddressId") REFERENCES "Addresses" ("Id");

-- JobApplications Foreign Keys
ALTER TABLE "JobApplications" ADD CONSTRAINT "FK_JobApplications_Freelancer" 
    FOREIGN KEY ("FreelancerUserId") REFERENCES "Users" ("Id");
ALTER TABLE "JobApplications" ADD CONSTRAINT "FK_JobApplications_Job" 
    FOREIGN KEY ("JobId") REFERENCES "Jobs" ("Id") ON DELETE CASCADE;

-- JobHires Foreign Keys
ALTER TABLE "JobHires" ADD CONSTRAINT "FK_Hirings_Jobs_JobId" 
    FOREIGN KEY ("JobId") REFERENCES "Jobs" ("Id") ON DELETE CASCADE;
ALTER TABLE "JobHires" ADD CONSTRAINT "FK_Hirings_Users_FreelancerId" 
    FOREIGN KEY ("FreelancerId") REFERENCES "Users" ("Id");
ALTER TABLE "JobHires" ADD CONSTRAINT "FK_Hirings_Users_HiredManagerId" 
    FOREIGN KEY ("HiredManagerId") REFERENCES "Users" ("Id") ON DELETE CASCADE;

-- JobQuestionnaireAnswers Foreign Keys
ALTER TABLE "JobQuestionnaireAnswers" ADD CONSTRAINT "FK_JobQuestionnaireAnswers_JobApplication" 
    FOREIGN KEY ("JobApplicationId") REFERENCES "JobApplications" ("Id") ON DELETE CASCADE;
ALTER TABLE "JobQuestionnaireAnswers" ADD CONSTRAINT "FK_JobQuestionnaireAnswers_Question" 
    FOREIGN KEY ("QuestionId") REFERENCES "JobQuestionnaires" ("Id");
ALTER TABLE "JobQuestionnaireAnswers" ADD CONSTRAINT "FK_JobQuestionnaireAnswers_User" 
    FOREIGN KEY ("UserId") REFERENCES "Users" ("Id");

-- JobQuestionnaires Foreign Keys
ALTER TABLE "JobQuestionnaires" ADD CONSTRAINT "FK_JobQuestionnaires_Job" 
    FOREIGN KEY ("JobId") REFERENCES "Jobs" ("Id") ON DELETE CASCADE;

-- Jobs Foreign Keys
ALTER TABLE "Jobs" ADD CONSTRAINT "FK_Jobs_Address" 
    FOREIGN KEY ("AddressId") REFERENCES "Addresses" ("Id");
ALTER TABLE "Jobs" ADD CONSTRAINT "FK_Jobs_Client" 
    FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id");
ALTER TABLE "Jobs" ADD CONSTRAINT "FK_Jobs_JobManager" 
    FOREIGN KEY ("JobManagerUserId") REFERENCES "Users" ("Id");

-- Notifications Foreign Keys
ALTER TABLE "Notifications" ADD CONSTRAINT "FK_Notifications_Users_UserId" 
    FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE;

-- PaymentMethods Foreign Keys
ALTER TABLE "PaymentMethods" ADD CONSTRAINT "FK_PaymentMethods_Client" 
    FOREIGN KEY ("client_id") REFERENCES "Clients" ("Id") ON DELETE SET NULL;
ALTER TABLE "PaymentMethods" ADD CONSTRAINT "FK_PaymentMethods_User" 
    FOREIGN KEY ("user_id") REFERENCES "Users" ("Id") ON DELETE CASCADE;

-- Payments Foreign Keys
ALTER TABLE "Payments" ADD CONSTRAINT "FK_Payments_Job" 
    FOREIGN KEY ("job_id") REFERENCES "Jobs" ("Id");
ALTER TABLE "Payments" ADD CONSTRAINT "FK_Payments_PaidByUser" 
    FOREIGN KEY ("paid_by_user_id") REFERENCES "Users" ("Id");
ALTER TABLE "Payments" ADD CONSTRAINT "FK_Payments_PaidToUser" 
    FOREIGN KEY ("paid_to_user_id") REFERENCES "Users" ("Id");
ALTER TABLE "Payments" ADD CONSTRAINT "FK_Payments_PaymentMethod" 
    FOREIGN KEY ("payment_method_id") REFERENCES "PaymentMethods" ("id");

-- TimeSheet Foreign Keys
ALTER TABLE "TimeSheet" ADD CONSTRAINT "FK_TimeSheet_Hiring" 
    FOREIGN KEY ("HiringId") REFERENCES "JobHires" ("Id");
ALTER TABLE "TimeSheet" ADD CONSTRAINT "FK_TimeSheet_User" 
    FOREIGN KEY ("UserId") REFERENCES "Users" ("Id");

-- TimeSheetLocation Foreign Keys
ALTER TABLE "TimeSheetLocation" ADD CONSTRAINT "FK_TimeSheetLocation_TimeSheet" 
    FOREIGN KEY ("TimeSheetId") REFERENCES "TimeSheet" ("Id");

-- UserDevices Foreign Keys
ALTER TABLE "UserDevices" ADD CONSTRAINT "FK_UserDevices_Users_UserId" 
    FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE;

-- Users Foreign Keys
ALTER TABLE "Users" ADD CONSTRAINT "FK_Users_Address" 
    FOREIGN KEY ("AddressId") REFERENCES "Addresses" ("Id");
ALTER TABLE "Users" ADD CONSTRAINT "FK_Users_DisabledByUser" 
    FOREIGN KEY ("DisabledByUserId") REFERENCES "Users" ("Id");

-- UserVendors Foreign Keys
ALTER TABLE "UserVendors" ADD CONSTRAINT "FK_UserVendors_UserId" 
    FOREIGN KEY ("UserId") REFERENCES "Users" ("Id");
ALTER TABLE "UserVendors" ADD CONSTRAINT "FK_UserVendors_VendorId" 
    FOREIGN KEY ("VendorId") REFERENCES "Vendors" ("Id");

-- Vendors Foreign Keys
ALTER TABLE "Vendors" ADD CONSTRAINT "FK_Vendors_User" 
    FOREIGN KEY ("UserId") REFERENCES "Users" ("Id");

-- VendorsRegistrants Foreign Keys
ALTER TABLE "VendorsRegistrants" ADD CONSTRAINT "FK_VendorsRegistrants_User" 
    FOREIGN KEY ("UserId") REFERENCES "Users" ("Id");
ALTER TABLE "VendorsRegistrants" ADD CONSTRAINT "FK_VendorsRegistrants_Vendor" 
    FOREIGN KEY ("VendorId") REFERENCES "Vendors" ("Id");

-- Ratings Foreign Keys
ALTER TABLE "Ratings" ADD CONSTRAINT "FK_Ratings_Jobs" 
    FOREIGN KEY ("JobId") REFERENCES "Jobs" ("Id");
ALTER TABLE "Ratings" ADD CONSTRAINT "FK_Ratings_JobHires" 
    FOREIGN KEY ("JobHireId") REFERENCES "JobHires" ("Id");
ALTER TABLE "Ratings" ADD CONSTRAINT "FK_Ratings_Caregivers" 
    FOREIGN KEY ("CaregiverId") REFERENCES "Users" ("Id");
ALTER TABLE "Ratings" ADD CONSTRAINT "FK_Ratings_Clients" 
    FOREIGN KEY ("FacilityId") REFERENCES "Clients" ("Id");
ALTER TABLE "Ratings" ADD CONSTRAINT "FK_Ratings_JobManagers" 
    FOREIGN KEY ("JobManagerId") REFERENCES "Users" ("Id");
ALTER TABLE "Ratings" ADD CONSTRAINT "FK_Ratings_Vendors" 
    FOREIGN KEY ("VendorId") REFERENCES "Vendors" ("Id");
ALTER TABLE "Ratings" ADD CONSTRAINT "FK_Ratings_RatingGivenByUser" 
    FOREIGN KEY ("RatingGivenByUserId") REFERENCES "Users" ("Id");
