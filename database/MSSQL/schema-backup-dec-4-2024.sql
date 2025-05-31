-- DROP SCHEMA gig_app;

CREATE SCHEMA gig_app;
-- gig_app_dev_db.gig_app.[__EFMigrationsHistory] definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.[__EFMigrationsHistory];

CREATE TABLE gig_app_dev_db.gig_app.[__EFMigrationsHistory] (
	MigrationId nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProductVersion nvarchar(32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
);


-- gig_app_dev_db.gig_app.sysdiagrams definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.sysdiagrams;

CREATE TABLE gig_app_dev_db.gig_app.sysdiagrams (
	name sysname COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	principal_id int NOT NULL,
	diagram_id int IDENTITY(1,1) NOT NULL,
	version int NULL,
	definition varbinary(MAX) NULL,
	CONSTRAINT PK__sysdiagr__C2B05B6143281170 PRIMARY KEY (diagram_id),
	CONSTRAINT UK_principal_name UNIQUE (principal_id,name)
);


-- gig_app_dev_db.gig_app.Users definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.Users;

CREATE TABLE gig_app_dev_db.gig_app.Users (
	Id bigint IDENTITY(1,1) NOT NULL,
	Auth0UserId nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Email nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	FirstName nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	LastName nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Disabled bit DEFAULT 0 NULL,
	DisabledByUserId bigint NULL,
	CreatedAt datetime2 DEFAULT getdate() NULL,
	UpdatedAt datetime2 DEFAULT getdate() NULL,
	Picture nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK__Users__3214EC072E9C31C9 PRIMARY KEY (Id),
	CONSTRAINT UQ__Users__1C8F42901DE32F1E UNIQUE (Auth0UserId),
	CONSTRAINT UQ__Users__A9D10534AEDC4F06 UNIQUE (Email),
	CONSTRAINT FK_Users_DisabledByUser FOREIGN KEY (DisabledByUserId) REFERENCES gig_app_dev_db.gig_app.Users(Id)
);


-- gig_app_dev_db.gig_app.Vendors definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.Vendors;

CREATE TABLE gig_app_dev_db.gig_app.Vendors (
	Id int IDENTITY(1,1) NOT NULL,
	UserId bigint NULL,
	ServicesOffered nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Certifications nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Status nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CreatedAt datetime2 DEFAULT getdate() NULL,
	CONSTRAINT PK__Vendors__3214EC07ABA49CF8 PRIMARY KEY (Id),
	CONSTRAINT FK__Vendors__UserId__01D345B0 FOREIGN KEY (UserId) REFERENCES gig_app_dev_db.gig_app.Users(Id)
);


-- gig_app_dev_db.gig_app.BankAccounts definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.BankAccounts;

CREATE TABLE gig_app_dev_db.gig_app.BankAccounts (
	Id int IDENTITY(1,1) NOT NULL,
	UserId bigint NOT NULL,
	BankName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	BankAccountType nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	BankAccountNumber nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	BankAccountName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	BankSwiftCode nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	BankCountry nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	status nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CreateAt datetime2 NOT NULL,
	CONSTRAINT PK_BankAccounts PRIMARY KEY (Id),
	CONSTRAINT FK_BankAccounts_Users_UserId FOREIGN KEY (UserId) REFERENCES gig_app_dev_db.gig_app.Users(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_BankAccounts_UserId ON gig_app.BankAccounts (  UserId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- gig_app_dev_db.gig_app.Companies definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.Companies;

CREATE TABLE gig_app_dev_db.gig_app.Companies (
	Id bigint IDENTITY(1,1) NOT NULL,
	Name nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	AdminUserId bigint NULL,
	CreatedAt datetime2 DEFAULT getdate() NULL,
	UpdatedAt datetime2 DEFAULT getdate() NULL,
	CONSTRAINT PK__Companie__3214EC07898B729E PRIMARY KEY (Id),
	CONSTRAINT FK_Companies_AdminUser FOREIGN KEY (AdminUserId) REFERENCES gig_app_dev_db.gig_app.Users(Id)
);


-- gig_app_dev_db.gig_app.Contracts definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.Contracts;

CREATE TABLE gig_app_dev_db.gig_app.Contracts (
	Id int IDENTITY(1,1) NOT NULL,
	VendorId int NULL,
	FacilityId bigint NULL,
	Terms nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Pricing decimal(10,2) NOT NULL,
	Status nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CreatedAt datetime2 DEFAULT getdate() NULL,
	CONSTRAINT PK__Contract__3214EC07303B18F7 PRIMARY KEY (Id),
	CONSTRAINT FK__Contracts__Facil__0697FACD FOREIGN KEY (FacilityId) REFERENCES gig_app_dev_db.gig_app.Companies(Id),
	CONSTRAINT FK__Contracts__Vendo__05A3D694 FOREIGN KEY (VendorId) REFERENCES gig_app_dev_db.gig_app.Vendors(Id)
);


-- gig_app_dev_db.gig_app.FacilityManagers definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.FacilityManagers;

CREATE TABLE gig_app_dev_db.gig_app.FacilityManagers (
	Id int IDENTITY(1,1) NOT NULL,
	UserId bigint NULL,
	FacilityId bigint NULL,
	CreatedAt datetime2 DEFAULT getdate() NULL,
	CONSTRAINT PK__Facility__3214EC078FD18450 PRIMARY KEY (Id),
	CONSTRAINT FK__FacilityM__Facil__0F2D40CE FOREIGN KEY (FacilityId) REFERENCES gig_app_dev_db.gig_app.Companies(Id),
	CONSTRAINT FK__FacilityM__UserI__0E391C95 FOREIGN KEY (UserId) REFERENCES gig_app_dev_db.gig_app.Users(Id)
);


-- gig_app_dev_db.gig_app.FreelancerLicenses definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.FreelancerLicenses;

CREATE TABLE gig_app_dev_db.gig_app.FreelancerLicenses (
	Id int IDENTITY(1,1) NOT NULL,
	FreelancerUserId bigint NULL,
	LicenseName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	LicenseNumber nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	IssuedBy nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	IssuedDate date NOT NULL,
	LicenseStatus nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	RejectionReason nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	LicenseFileType nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	LicenseFileUrl nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CreatedAt datetime2 DEFAULT getdate() NULL,
	CONSTRAINT PK__Freelanc__3214EC07A91848B0 PRIMARY KEY (Id),
	CONSTRAINT FK__Freelance__Freel__6AEFE058 FOREIGN KEY (FreelancerUserId) REFERENCES gig_app_dev_db.gig_app.Users(Id)
);


-- gig_app_dev_db.gig_app.Invoices definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.Invoices;

CREATE TABLE gig_app_dev_db.gig_app.Invoices (
	Id int IDENTITY(1,1) NOT NULL,
	ContractId int NULL,
	Amount decimal(10,2) NOT NULL,
	Status nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CreatedAt datetime2 DEFAULT getdate() NULL,
	CONSTRAINT PK__Invoices__3214EC078C6841DE PRIMARY KEY (Id),
	CONSTRAINT FK__Invoices__Contra__0A688BB1 FOREIGN KEY (ContractId) REFERENCES gig_app_dev_db.gig_app.Contracts(Id)
);


-- gig_app_dev_db.gig_app.Notifications definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.Notifications;

CREATE TABLE gig_app_dev_db.gig_app.Notifications (
	Id int IDENTITY(1,1) NOT NULL,
	UserId bigint NOT NULL,
	Title nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Description nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Image] nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Type] nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	NotificationTime datetime2 NOT NULL,
	HasViewed bit NOT NULL,
	CreatedAt datetime2 NOT NULL,
	CONSTRAINT PK_Notifications PRIMARY KEY (Id),
	CONSTRAINT FK_Notifications_Users_UserId FOREIGN KEY (UserId) REFERENCES gig_app_dev_db.gig_app.Users(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Notifications_UserId ON gig_app.Notifications (  UserId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- gig_app_dev_db.gig_app.PaymentMethods definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.PaymentMethods;

CREATE TABLE gig_app_dev_db.gig_app.PaymentMethods (
	id int IDENTITY(1,1) NOT NULL,
	user_id bigint NOT NULL,
	company_id bigint NULL,
	card_last_four varchar(4) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	card_brand varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	exp_month int NOT NULL,
	exp_year int NOT NULL,
	cardholder_name varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	stripe_payment_method_id varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	stripe_card_id varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	is_default bit DEFAULT 0 NULL,
	status varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS DEFAULT 'active' NULL,
	created_at datetime DEFAULT getdate() NULL,
	updated_at datetime DEFAULT getdate() NULL,
	CONSTRAINT PK__PaymentM__3213E83FD732D903 PRIMARY KEY (id),
	CONSTRAINT UQ__PaymentM__B61B6229B5DBE7E7 UNIQUE (stripe_card_id),
	CONSTRAINT UQ__PaymentM__D66D16C6A4FB918F UNIQUE (stripe_payment_method_id),
	CONSTRAINT FK__PaymentMe__compa__5772F790 FOREIGN KEY (company_id) REFERENCES gig_app_dev_db.gig_app.Companies(Id) ON DELETE SET NULL,
	CONSTRAINT FK__PaymentMe__user___567ED357 FOREIGN KEY (user_id) REFERENCES gig_app_dev_db.gig_app.Users(Id) ON DELETE CASCADE
);


-- gig_app_dev_db.gig_app.UserDevices definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.UserDevices;

CREATE TABLE gig_app_dev_db.gig_app.UserDevices (
	Id int IDENTITY(1,1) NOT NULL,
	UserId bigint NOT NULL,
	FirebaseToken nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DeviceInfo nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CreatedAt datetime2 DEFAULT '0001-01-01T00:00:00.0000000' NOT NULL,
	CONSTRAINT PK_UserDevices PRIMARY KEY (Id),
	CONSTRAINT FK_UserDevices_Users_UserId FOREIGN KEY (UserId) REFERENCES gig_app_dev_db.gig_app.Users(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_UserDevices_UserId ON gig_app.UserDevices (  UserId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- gig_app_dev_db.gig_app.Addresses definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.Addresses;

CREATE TABLE gig_app_dev_db.gig_app.Addresses (
	Id bigint IDENTITY(1,1) NOT NULL,
	Label nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	StreetAddress1 nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	StreetAddress2 nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	UserId bigint NULL,
	CompanyId bigint NULL,
	City nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	StateProvince nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	PostalCode nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Latitude decimal(10,7) NULL,
	Longitude decimal(10,7) NULL,
	Country nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CreatedAt datetime2 DEFAULT getdate() NULL,
	UpdatedAt datetime2 DEFAULT getdate() NULL,
	CONSTRAINT PK__Addresse__3214EC079838E5E8 PRIMARY KEY (Id),
	CONSTRAINT FK_Addresses_Company FOREIGN KEY (CompanyId) REFERENCES gig_app_dev_db.gig_app.Companies(Id) ON DELETE CASCADE,
	CONSTRAINT FK_Addresses_User FOREIGN KEY (UserId) REFERENCES gig_app_dev_db.gig_app.Users(Id)
);


-- gig_app_dev_db.gig_app.CompanyLocations definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.CompanyLocations;

CREATE TABLE gig_app_dev_db.gig_app.CompanyLocations (
	Id bigint IDENTITY(1,1) NOT NULL,
	Name nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	AddressId bigint NULL,
	LocationName nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CompanyId bigint NULL,
	CreatedByUserId bigint NULL,
	CreatedAt datetime2 DEFAULT getdate() NULL,
	UpdatedAt datetime2 DEFAULT getdate() NULL,
	CONSTRAINT PK__CompanyL__3214EC0747145E5C PRIMARY KEY (Id),
	CONSTRAINT FK_CompanyLocations_Address FOREIGN KEY (AddressId) REFERENCES gig_app_dev_db.gig_app.Addresses(Id),
	CONSTRAINT FK_CompanyLocations_Company FOREIGN KEY (CompanyId) REFERENCES gig_app_dev_db.gig_app.Companies(Id),
	CONSTRAINT FK_CompanyLocations_CreatedByUser FOREIGN KEY (CreatedByUserId) REFERENCES gig_app_dev_db.gig_app.Users(Id)
);


-- gig_app_dev_db.gig_app.Freelancers definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.Freelancers;

CREATE TABLE gig_app_dev_db.gig_app.Freelancers (
	UserId int NOT NULL,
	AddressId bigint NULL,
	LicenseInfo nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	BankAccountInfo nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CreatedAt datetime2 DEFAULT getdate() NULL,
	CONSTRAINT PK__Freelanc__1788CC4C296A1722 PRIMARY KEY (UserId),
	CONSTRAINT FK__Freelance__Addre__671F4F74 FOREIGN KEY (AddressId) REFERENCES gig_app_dev_db.gig_app.Addresses(Id)
);


-- gig_app_dev_db.gig_app.Jobs definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.Jobs;

CREATE TABLE gig_app_dev_db.gig_app.Jobs (
	Id bigint IDENTITY(1,1) NOT NULL,
	Title nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Description nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Requirements nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Date] date NOT NULL,
	[Time] time NOT NULL,
	Rate decimal(10,2) NOT NULL,
	Status nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	JobManagerUserId bigint NULL,
	AddressId bigint NULL,
	FacilityId bigint NULL,
	CreatedAt datetime2 DEFAULT getdate() NULL,
	UpdatedAt datetime2 DEFAULT getdate() NULL,
	LicenseRequirments nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK__Jobs__3214EC0772D015A5 PRIMARY KEY (Id),
	CONSTRAINT FK_Jobs_Address FOREIGN KEY (AddressId) REFERENCES gig_app_dev_db.gig_app.Addresses(Id),
	CONSTRAINT FK_Jobs_Facility FOREIGN KEY (FacilityId) REFERENCES gig_app_dev_db.gig_app.Companies(Id),
	CONSTRAINT FK_Jobs_JobManager FOREIGN KEY (JobManagerUserId) REFERENCES gig_app_dev_db.gig_app.Users(Id)
);


-- gig_app_dev_db.gig_app.Payments definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.Payments;

CREATE TABLE gig_app_dev_db.gig_app.Payments (
	id int IDENTITY(1,1) NOT NULL,
	job_id bigint NULL,
	payment_method_id int NULL,
	amount decimal(10,2) NOT NULL,
	status varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	payment_type varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	escrow_status varchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	stripe_payment_intent_id varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	stripe_transfer_id varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	description text COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	failure_reason text COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	paid_by_user_id bigint NULL,
	paid_to_user_id bigint NULL,
	created_at datetime DEFAULT getdate() NULL,
	updated_at datetime DEFAULT getdate() NULL,
	CONSTRAINT PK__Payments__3213E83F3B51BECD PRIMARY KEY (id),
	CONSTRAINT UQ__Payments__051F88970932CADA UNIQUE (stripe_payment_intent_id),
	CONSTRAINT UQ__Payments__52C59C5278B4C9FF UNIQUE (stripe_transfer_id),
	CONSTRAINT FK__Payments__job_id__6C6E1476 FOREIGN KEY (job_id) REFERENCES gig_app_dev_db.gig_app.Jobs(Id),
	CONSTRAINT FK__Payments__paid_b__6E565CE8 FOREIGN KEY (paid_by_user_id) REFERENCES gig_app_dev_db.gig_app.Users(Id),
	CONSTRAINT FK__Payments__paid_t__6F4A8121 FOREIGN KEY (paid_to_user_id) REFERENCES gig_app_dev_db.gig_app.Users(Id),
	CONSTRAINT FK__Payments__paymen__6D6238AF FOREIGN KEY (payment_method_id) REFERENCES gig_app_dev_db.gig_app.PaymentMethods(id)
);


-- gig_app_dev_db.gig_app.JobApplications definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.JobApplications;

CREATE TABLE gig_app_dev_db.gig_app.JobApplications (
	Id bigint IDENTITY(1,1) NOT NULL,
	JobId bigint NULL,
	FreelancerUserId bigint NULL,
	JobApplicationStatus nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	WithdrawalStatus nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	WithdrawalReason nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	WithdrawalDate datetime2 NULL,
	Proposal nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProposalHourlyRate decimal(10,2) NULL,
	Viewed bit DEFAULT 0 NULL,
	CreatedAt datetime2 DEFAULT getdate() NULL,
	CONSTRAINT PK__JobAppli__3214EC072A5EE321 PRIMARY KEY (Id),
	CONSTRAINT FK_JobApplications_Freelancer FOREIGN KEY (FreelancerUserId) REFERENCES gig_app_dev_db.gig_app.Users(Id),
	CONSTRAINT FK_JobApplications_Job FOREIGN KEY (JobId) REFERENCES gig_app_dev_db.gig_app.Jobs(Id) ON DELETE CASCADE
);


-- gig_app_dev_db.gig_app.JobHires definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.JobHires;

CREATE TABLE gig_app_dev_db.gig_app.JobHires (
	Id int IDENTITY(1,1) NOT NULL,
	JobId bigint NOT NULL,
	FreelancerId bigint NULL,
	HiredManagerId bigint NOT NULL,
	Status nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Note nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	StartTime datetime2 NOT NULL,
	EndTime datetime2 NOT NULL,
	CreatedAt datetime2 NOT NULL,
	CONSTRAINT PK_Hirings PRIMARY KEY (Id),
	CONSTRAINT FK_Hirings_Jobs_JobId FOREIGN KEY (JobId) REFERENCES gig_app_dev_db.gig_app.Jobs(Id) ON DELETE CASCADE,
	CONSTRAINT FK_Hirings_Users_FreelancerId FOREIGN KEY (FreelancerId) REFERENCES gig_app_dev_db.gig_app.Users(Id),
	CONSTRAINT FK_Hirings_Users_HiredManagerId FOREIGN KEY (HiredManagerId) REFERENCES gig_app_dev_db.gig_app.Users(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Hirings_FreelancerId ON gig_app.JobHires (  FreelancerId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Hirings_HiredManagerId ON gig_app.JobHires (  HiredManagerId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Hirings_JobId ON gig_app.JobHires (  JobId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- gig_app_dev_db.gig_app.JobQuestionnaires definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.JobQuestionnaires;

CREATE TABLE gig_app_dev_db.gig_app.JobQuestionnaires (
	Id bigint IDENTITY(1,1) NOT NULL,
	JobId bigint NULL,
	Question nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CreatedAt datetime2 DEFAULT getdate() NULL,
	CONSTRAINT PK__JobQuest__3214EC076E47F9D4 PRIMARY KEY (Id),
	CONSTRAINT FK_JobQuestionnaires_Job FOREIGN KEY (JobId) REFERENCES gig_app_dev_db.gig_app.Jobs(Id) ON DELETE CASCADE
);


-- gig_app_dev_db.gig_app.TimeSheet definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.TimeSheet;

CREATE TABLE gig_app_dev_db.gig_app.TimeSheet (
	Id bigint IDENTITY(1,1) NOT NULL,
	HiringId int NOT NULL,
	UserId bigint NOT NULL,
	ClockIn datetime NOT NULL,
	ClockOut datetime NULL,
	TimeSheetNotes text COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Status varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	TimeSheetApprovalStatus varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK__TimeShee__3214EC070350DA26 PRIMARY KEY (Id),
	CONSTRAINT FK__TimeSheet__Hirin__1881A0DE FOREIGN KEY (HiringId) REFERENCES gig_app_dev_db.gig_app.JobHires(Id),
	CONSTRAINT FK__TimeSheet__UserI__1975C517 FOREIGN KEY (UserId) REFERENCES gig_app_dev_db.gig_app.Users(Id)
);


-- gig_app_dev_db.gig_app.TimeSheetLocation definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.TimeSheetLocation;

CREATE TABLE gig_app_dev_db.gig_app.TimeSheetLocation (
	Id bigint IDENTITY(1,1) NOT NULL,
	TimeSheetId bigint NOT NULL,
	LocationCapturedDateTime datetime NOT NULL,
	Location geography NOT NULL,
	CONSTRAINT PK__TimeShee__3214EC079ECDC2B4 PRIMARY KEY (Id),
	CONSTRAINT FK__TimeSheet__TimeS__1C5231C2 FOREIGN KEY (TimeSheetId) REFERENCES gig_app_dev_db.gig_app.TimeSheet(Id)
);


-- gig_app_dev_db.gig_app.Dispute definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.Dispute;

CREATE TABLE gig_app_dev_db.gig_app.Dispute (
	Id bigint IDENTITY(1,1) NOT NULL,
	[Type] nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Reason nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Description nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Attachment nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	HiredJobId int NULL,
	TimeSheetId bigint NULL,
	Status nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProcessedByUserId bigint NULL,
	ProcessedDateTime datetime NULL,
	CONSTRAINT PK__Dispute__3214EC07638B5AF8 PRIMARY KEY (Id),
	CONSTRAINT FK__Dispute__HiredJo__16644E42 FOREIGN KEY (HiredJobId) REFERENCES gig_app_dev_db.gig_app.JobHires(Id),
	CONSTRAINT FK__Dispute__Process__184C96B4 FOREIGN KEY (ProcessedByUserId) REFERENCES gig_app_dev_db.gig_app.Users(Id),
	CONSTRAINT FK__Dispute__TimeShe__1758727B FOREIGN KEY (TimeSheetId) REFERENCES gig_app_dev_db.gig_app.TimeSheet(Id)
);


-- gig_app_dev_db.gig_app.DisputeAction definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.DisputeAction;

CREATE TABLE gig_app_dev_db.gig_app.DisputeAction (
	Id bigint IDENTITY(1,1) NOT NULL,
	DisputeId bigint NOT NULL,
	ProcessedByUserId bigint NOT NULL,
	ProcessedDateTime datetime NOT NULL,
	ActionMessage nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ActionStatus nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK__DisputeA__3214EC07EBD2D4B2 PRIMARY KEY (Id),
	CONSTRAINT FK__DisputeAc__Dispu__1B29035F FOREIGN KEY (DisputeId) REFERENCES gig_app_dev_db.gig_app.Dispute(Id),
	CONSTRAINT FK__DisputeAc__Proce__1C1D2798 FOREIGN KEY (ProcessedByUserId) REFERENCES gig_app_dev_db.gig_app.Users(Id)
);


-- gig_app_dev_db.gig_app.JobQuestionnaireAnswers definition

-- Drop table

-- DROP TABLE gig_app_dev_db.gig_app.JobQuestionnaireAnswers;

CREATE TABLE gig_app_dev_db.gig_app.JobQuestionnaireAnswers (
	Id int IDENTITY(1,1) NOT NULL,
	JobApplicationId bigint NULL,
	QuestionId bigint NULL,
	UserId bigint NULL,
	Answer nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CreatedAt datetime2 DEFAULT getdate() NULL,
	CONSTRAINT PK__JobQuest__3214EC07416D966B PRIMARY KEY (Id),
	CONSTRAINT FK__JobQuesti__JobAp__793DFFAF FOREIGN KEY (JobApplicationId) REFERENCES gig_app_dev_db.gig_app.JobApplications(Id) ON DELETE CASCADE,
	CONSTRAINT FK__JobQuesti__Quest__7A3223E8 FOREIGN KEY (QuestionId) REFERENCES gig_app_dev_db.gig_app.JobQuestionnaires(Id),
	CONSTRAINT FK__JobQuesti__UserI__7B264821 FOREIGN KEY (UserId) REFERENCES gig_app_dev_db.gig_app.Users(Id)
);