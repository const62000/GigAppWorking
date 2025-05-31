/****** Object:  Table [dbo].[Addresses]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Addresses](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Label] [nvarchar](255) NULL,
	[StreetAddress1] [nvarchar](255) NOT NULL,
	[StreetAddress2] [nvarchar](255) NULL,
	[UserId] [bigint] NULL,
	[CompanyId] [bigint] NULL,
	[City] [nvarchar](200) NOT NULL,
	[StateProvince] [nvarchar](100) NULL,
	[PostalCode] [nvarchar](20) NULL,
	[Latitude] [decimal](10, 7) NULL,
	[Longitude] [decimal](10, 7) NULL,
	[Country] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BankAccounts]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BankAccounts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[BankName] [nvarchar](max) NOT NULL,
	[BankAccountType] [nvarchar](max) NOT NULL,
	[BankAccountNumber] [nvarchar](max) NOT NULL,
	[BankAccountName] [nvarchar](max) NOT NULL,
	[BankSwiftCode] [nvarchar](max) NOT NULL,
	[BankCountry] [nvarchar](max) NOT NULL,
	[status] [nvarchar](max) NOT NULL,
	[CreateAt] [datetime2](7) NOT NULL,
	[StripeBankAccountId] [varchar](255) NULL,
	[StripeCustomerId] [varchar](255) NULL,
 CONSTRAINT [PK_BankAccounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Companies]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[AdminUserId] [bigint] NULL,
	[CreatedAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompanyLocations]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyLocations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[AddressId] [bigint] NULL,
	[LocationName] [nvarchar](20) NOT NULL,
	[CompanyId] [bigint] NULL,
	[CreatedByUserId] [bigint] NULL,
	[CreatedAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contracts]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contracts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VendorId] [int] NULL,
	[FacilityId] [bigint] NULL,
	[Terms] [nvarchar](max) NOT NULL,
	[Pricing] [decimal](10, 2) NOT NULL,
	[Status] [nvarchar](20) NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dispute]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dispute](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Reason] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Attachment] [nvarchar](255) NULL,
	[HiredJobId] [int] NULL,
	[TimeSheetId] [bigint] NULL,
	[Status] [nvarchar](50) NOT NULL,
	[ProcessedByUserId] [bigint] NULL,
	[ProcessedDateTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DisputeAction]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DisputeAction](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DisputeId] [bigint] NOT NULL,
	[ProcessedByUserId] [bigint] NOT NULL,
	[ProcessedDateTime] [datetime] NOT NULL,
	[ActionMessage] [nvarchar](max) NOT NULL,
	[ActionStatus] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FacilityManagers]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FacilityManagers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NULL,
	[FacilityId] [bigint] NULL,
	[CreatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FreelancerLicenses]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FreelancerLicenses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FreelancerUserId] [bigint] NULL,
	[LicenseName] [nvarchar](max) NULL,
	[LicenseNumber] [nvarchar](max) NULL,
	[IssuedBy] [nvarchar](100) NOT NULL,
	[IssuedDate] [date] NOT NULL,
	[LicenseStatus] [nvarchar](20) NOT NULL,
	[RejectionReason] [nvarchar](max) NULL,
	[LicenseFileType] [nvarchar](20) NULL,
	[LicenseFileUrl] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Freelancers]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Freelancers](
	[UserId] [int] NOT NULL,
	[AddressId] [bigint] NULL,
	[LicenseInfo] [nvarchar](max) NULL,
	[BankAccountInfo] [nvarchar](max) NULL,
	[CreatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoices]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContractId] [int] NULL,
	[Amount] [decimal](10, 2) NOT NULL,
	[Status] [nvarchar](20) NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobApplications]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobApplications](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[JobId] [bigint] NULL,
	[FreelancerUserId] [bigint] NULL,
	[JobApplicationStatus] [nvarchar](20) NOT NULL,
	[WithdrawalStatus] [nvarchar](20) NULL,
	[WithdrawalReason] [nvarchar](max) NULL,
	[WithdrawalDate] [datetime2](7) NULL,
	[Proposal] [nvarchar](max) NOT NULL,
	[ProposalHourlyRate] [decimal](10, 2) NULL,
	[Viewed] [bit] NULL,
	[CreatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobHires]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobHires](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [bigint] NOT NULL,
	[FreelancerId] [bigint] NULL,
	[HiredManagerId] [bigint] NOT NULL,
	[Status] [nvarchar](max) NOT NULL,
	[Note] [nvarchar](max) NULL,
	[StartTime] [datetime2](7) NOT NULL,
	[EndTime] [datetime2](7) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Hirings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobQuestionnaireAnswers]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobQuestionnaireAnswers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobApplicationId] [bigint] NULL,
	[QuestionId] [bigint] NULL,
	[UserId] [bigint] NULL,
	[Answer] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobQuestionnaires]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobQuestionnaires](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[JobId] [bigint] NULL,
	[Question] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Jobs]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Jobs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Requirements] [nvarchar](max) NULL,
	[Date] [date] NOT NULL,
	[Time] [time](7) NOT NULL,
	[Rate] [decimal](10, 2) NOT NULL,
	[Status] [nvarchar](20) NOT NULL,
	[JobManagerUserId] [bigint] NULL,
	[AddressId] [bigint] NULL,
	[FacilityId] [bigint] NULL,
	[CreatedAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[LicenseRequirments] [nvarchar](max) NULL,
	[HoursPerWeek] [int] NULL,
	[JobType] [int] NULL,
	[PaymentIntentId] [nvarchar](255) NULL,
	[PaymentMethodId] [int] NULL,
	[StartedDate] [datetime2](7) NULL,
	[EndedDate] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Image] [nvarchar](max) NULL,
	[Type] [nvarchar](max) NULL,
	[NotificationTime] [datetime2](7) NOT NULL,
	[HasViewed] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethods]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethods](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [bigint] NOT NULL,
	[company_id] [bigint] NULL,
	[card_last_four] [varchar](4) NOT NULL,
	[card_brand] [varchar](50) NOT NULL,
	[exp_month] [int] NOT NULL,
	[exp_year] [int] NOT NULL,
	[cardholder_name] [varchar](255) NOT NULL,
	[stripe_payment_method_id] [varchar](255) NOT NULL,
	[stripe_card_id] [varchar](255) NOT NULL,
	[is_default] [bit] NULL,
	[status] [varchar](20) NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
	[StripeCustomerId] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[stripe_card_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[stripe_payment_method_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[job_id] [bigint] NULL,
	[payment_method_id] [int] NULL,
	[amount] [decimal](10, 2) NOT NULL,
	[status] [varchar](20) NOT NULL,
	[payment_type] [varchar](20) NOT NULL,
	[escrow_status] [varchar](20) NULL,
	[stripe_payment_intent_id] [varchar](255) NULL,
	[stripe_transfer_id] [varchar](255) NULL,
	[description] [text] NULL,
	[failure_reason] [text] NULL,
	[paid_by_user_id] [bigint] NULL,
	[paid_to_user_id] [bigint] NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[stripe_payment_intent_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[stripe_transfer_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TimeSheet]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TimeSheet](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[HiringId] [int] NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ClockIn] [datetime] NOT NULL,
	[ClockOut] [datetime] NULL,
	[TimeSheetNotes] [text] NULL,
	[Status] [varchar](50) NOT NULL,
	[TimeSheetApprovalStatus] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TimeSheetLocation]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TimeSheetLocation](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TimeSheetId] [bigint] NOT NULL,
	[LocationCapturedDateTime] [datetime] NOT NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserDevices]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserDevices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[FirebaseToken] [nvarchar](max) NOT NULL,
	[DeviceInfo] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_UserDevices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Auth0UserId] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NOT NULL,
	[FirstName] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NULL,
	[Disabled] [bit] NULL,
	[DisabledByUserId] [bigint] NULL,
	[CreatedAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[Picture] [nvarchar](max) NULL,
	[StripeAccountId] [nvarchar](255) NULL,
	[StripeCustomerId] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Auth0UserId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserVendors]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserVendors](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[VendorId] [int] NOT NULL,
 CONSTRAINT [PK_UserVendors] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[VendorId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vendors]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vendors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NULL,
	[Name] [nvarchar](max) NULL,
	[ServicesOffered] [nvarchar](max) NULL,
	[Certifications] [nvarchar](max) NULL,
	[Status] [nvarchar](20) NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VendorsRegistrants]    Script Date: 1/24/2025 6:45:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VendorsRegistrants](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[VendorId] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_VendorsRegistrants] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Addresses] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Addresses] ADD  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Companies] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Companies] ADD  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[CompanyLocations] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[CompanyLocations] ADD  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Contracts] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[FacilityManagers] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[FreelancerLicenses] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Freelancers] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Invoices] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[JobApplications] ADD  DEFAULT ((0)) FOR [Viewed]
GO
ALTER TABLE [dbo].[JobApplications] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[JobQuestionnaireAnswers] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[JobQuestionnaires] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Jobs] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Jobs] ADD  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[PaymentMethods] ADD  DEFAULT ((0)) FOR [is_default]
GO
ALTER TABLE [dbo].[PaymentMethods] ADD  DEFAULT ('active') FOR [status]
GO
ALTER TABLE [dbo].[PaymentMethods] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[PaymentMethods] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[Payments] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Payments] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[UserDevices] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [Disabled]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Vendors] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[VendorsRegistrants] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Addresses]  WITH NOCHECK ADD  CONSTRAINT [FK_Addresses_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Addresses] NOCHECK CONSTRAINT [FK_Addresses_Company]
GO
ALTER TABLE [dbo].[Addresses]  WITH NOCHECK ADD  CONSTRAINT [FK_Addresses_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Addresses] NOCHECK CONSTRAINT [FK_Addresses_User]
GO
ALTER TABLE [dbo].[BankAccounts]  WITH NOCHECK ADD  CONSTRAINT [FK_BankAccounts_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BankAccounts] NOCHECK CONSTRAINT [FK_BankAccounts_Users_UserId]
GO
ALTER TABLE [dbo].[Companies]  WITH NOCHECK ADD  CONSTRAINT [FK_Companies_AdminUser] FOREIGN KEY([AdminUserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Companies] NOCHECK CONSTRAINT [FK_Companies_AdminUser]
GO
ALTER TABLE [dbo].[CompanyLocations]  WITH NOCHECK ADD  CONSTRAINT [FK_CompanyLocations_Address] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Addresses] ([Id])
GO
ALTER TABLE [dbo].[CompanyLocations] NOCHECK CONSTRAINT [FK_CompanyLocations_Address]
GO
ALTER TABLE [dbo].[CompanyLocations]  WITH NOCHECK ADD  CONSTRAINT [FK_CompanyLocations_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[CompanyLocations] NOCHECK CONSTRAINT [FK_CompanyLocations_Company]
GO
ALTER TABLE [dbo].[CompanyLocations]  WITH NOCHECK ADD  CONSTRAINT [FK_CompanyLocations_CreatedByUser] FOREIGN KEY([CreatedByUserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[CompanyLocations] NOCHECK CONSTRAINT [FK_CompanyLocations_CreatedByUser]
GO
ALTER TABLE [dbo].[Contracts]  WITH NOCHECK ADD FOREIGN KEY([FacilityId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[Contracts]  WITH NOCHECK ADD FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendors] ([Id])
GO
ALTER TABLE [dbo].[Dispute]  WITH NOCHECK ADD FOREIGN KEY([HiredJobId])
REFERENCES [dbo].[JobHires] ([Id])
GO
ALTER TABLE [dbo].[Dispute]  WITH NOCHECK ADD FOREIGN KEY([ProcessedByUserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Dispute]  WITH NOCHECK ADD FOREIGN KEY([TimeSheetId])
REFERENCES [dbo].[TimeSheet] ([Id])
GO
ALTER TABLE [dbo].[DisputeAction]  WITH NOCHECK ADD FOREIGN KEY([DisputeId])
REFERENCES [dbo].[Dispute] ([Id])
GO
ALTER TABLE [dbo].[DisputeAction]  WITH NOCHECK ADD FOREIGN KEY([ProcessedByUserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[FacilityManagers]  WITH NOCHECK ADD FOREIGN KEY([FacilityId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[FacilityManagers]  WITH NOCHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[FreelancerLicenses]  WITH NOCHECK ADD FOREIGN KEY([FreelancerUserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Freelancers]  WITH NOCHECK ADD FOREIGN KEY([AddressId])
REFERENCES [dbo].[Addresses] ([Id])
GO
ALTER TABLE [dbo].[Invoices]  WITH NOCHECK ADD FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contracts] ([Id])
GO
ALTER TABLE [dbo].[JobApplications]  WITH NOCHECK ADD  CONSTRAINT [FK_JobApplications_Freelancer] FOREIGN KEY([FreelancerUserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[JobApplications] NOCHECK CONSTRAINT [FK_JobApplications_Freelancer]
GO
ALTER TABLE [dbo].[JobApplications]  WITH NOCHECK ADD  CONSTRAINT [FK_JobApplications_Job] FOREIGN KEY([JobId])
REFERENCES [dbo].[Jobs] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[JobApplications] NOCHECK CONSTRAINT [FK_JobApplications_Job]
GO
ALTER TABLE [dbo].[JobHires]  WITH NOCHECK ADD  CONSTRAINT [FK_Hirings_Jobs_JobId] FOREIGN KEY([JobId])
REFERENCES [dbo].[Jobs] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[JobHires] NOCHECK CONSTRAINT [FK_Hirings_Jobs_JobId]
GO
ALTER TABLE [dbo].[JobHires]  WITH NOCHECK ADD  CONSTRAINT [FK_Hirings_Users_FreelancerId] FOREIGN KEY([FreelancerId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[JobHires] NOCHECK CONSTRAINT [FK_Hirings_Users_FreelancerId]
GO
ALTER TABLE [dbo].[JobHires]  WITH NOCHECK ADD  CONSTRAINT [FK_Hirings_Users_HiredManagerId] FOREIGN KEY([HiredManagerId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[JobHires] NOCHECK CONSTRAINT [FK_Hirings_Users_HiredManagerId]
GO
ALTER TABLE [dbo].[JobQuestionnaireAnswers]  WITH NOCHECK ADD  CONSTRAINT [FK__JobQuesti__JobAp__793DFFAF] FOREIGN KEY([JobApplicationId])
REFERENCES [dbo].[JobApplications] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[JobQuestionnaireAnswers] NOCHECK CONSTRAINT [FK__JobQuesti__JobAp__793DFFAF]
GO
ALTER TABLE [dbo].[JobQuestionnaireAnswers]  WITH NOCHECK ADD  CONSTRAINT [FK__JobQuesti__Quest__7A3223E8] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[JobQuestionnaires] ([Id])
GO
ALTER TABLE [dbo].[JobQuestionnaireAnswers] NOCHECK CONSTRAINT [FK__JobQuesti__Quest__7A3223E8]
GO
ALTER TABLE [dbo].[JobQuestionnaireAnswers]  WITH NOCHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[JobQuestionnaires]  WITH NOCHECK ADD  CONSTRAINT [FK_JobQuestionnaires_Job] FOREIGN KEY([JobId])
REFERENCES [dbo].[Jobs] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[JobQuestionnaires] NOCHECK CONSTRAINT [FK_JobQuestionnaires_Job]
GO
ALTER TABLE [dbo].[Jobs]  WITH NOCHECK ADD  CONSTRAINT [FK_Jobs_Address] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Addresses] ([Id])
GO
ALTER TABLE [dbo].[Jobs] NOCHECK CONSTRAINT [FK_Jobs_Address]
GO
ALTER TABLE [dbo].[Jobs]  WITH NOCHECK ADD  CONSTRAINT [FK_Jobs_Facility] FOREIGN KEY([FacilityId])
REFERENCES [dbo].[Companies] ([Id])
GO
ALTER TABLE [dbo].[Jobs] NOCHECK CONSTRAINT [FK_Jobs_Facility]
GO
ALTER TABLE [dbo].[Jobs]  WITH NOCHECK ADD  CONSTRAINT [FK_Jobs_JobManager] FOREIGN KEY([JobManagerUserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Jobs] NOCHECK CONSTRAINT [FK_Jobs_JobManager]
GO
ALTER TABLE [dbo].[Notifications]  WITH NOCHECK ADD  CONSTRAINT [FK_Notifications_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Notifications] NOCHECK CONSTRAINT [FK_Notifications_Users_UserId]
GO
ALTER TABLE [dbo].[PaymentMethods]  WITH NOCHECK ADD  CONSTRAINT [FK__PaymentMe__compa__5772F790] FOREIGN KEY([company_id])
REFERENCES [dbo].[Companies] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[PaymentMethods] NOCHECK CONSTRAINT [FK__PaymentMe__compa__5772F790]
GO
ALTER TABLE [dbo].[PaymentMethods]  WITH NOCHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Payments]  WITH NOCHECK ADD FOREIGN KEY([job_id])
REFERENCES [dbo].[Jobs] ([Id])
GO
ALTER TABLE [dbo].[Payments]  WITH NOCHECK ADD FOREIGN KEY([paid_by_user_id])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Payments]  WITH NOCHECK ADD FOREIGN KEY([paid_to_user_id])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Payments]  WITH NOCHECK ADD FOREIGN KEY([payment_method_id])
REFERENCES [dbo].[PaymentMethods] ([id])
GO
ALTER TABLE [dbo].[TimeSheet]  WITH NOCHECK ADD FOREIGN KEY([HiringId])
REFERENCES [dbo].[JobHires] ([Id])
GO
ALTER TABLE [dbo].[TimeSheet]  WITH NOCHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[TimeSheetLocation]  WITH NOCHECK ADD FOREIGN KEY([TimeSheetId])
REFERENCES [dbo].[TimeSheet] ([Id])
GO
ALTER TABLE [dbo].[UserDevices]  WITH NOCHECK ADD  CONSTRAINT [FK_UserDevices_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserDevices] NOCHECK CONSTRAINT [FK_UserDevices_Users_UserId]
GO
ALTER TABLE [dbo].[Users]  WITH NOCHECK ADD  CONSTRAINT [FK_Users_DisabledByUser] FOREIGN KEY([DisabledByUserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Users] NOCHECK CONSTRAINT [FK_Users_DisabledByUser]
GO
ALTER TABLE [dbo].[UserVendors]  WITH CHECK ADD  CONSTRAINT [FK_UserVendors_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserVendors] CHECK CONSTRAINT [FK_UserVendors_UserId]
GO
ALTER TABLE [dbo].[UserVendors]  WITH CHECK ADD  CONSTRAINT [FK_UserVendors_VendorId] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendors] ([Id])
GO
ALTER TABLE [dbo].[UserVendors] CHECK CONSTRAINT [FK_UserVendors_VendorId]
GO
ALTER TABLE [dbo].[Vendors]  WITH NOCHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[VendorsRegistrants]  WITH CHECK ADD  CONSTRAINT [FK_VendorsRegistrants_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[VendorsRegistrants] CHECK CONSTRAINT [FK_VendorsRegistrants_User]
GO
ALTER TABLE [dbo].[VendorsRegistrants]  WITH CHECK ADD  CONSTRAINT [FK_VendorsRegistrants_Vendor] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendors] ([Id])
GO
ALTER TABLE [dbo].[VendorsRegistrants] CHECK CONSTRAINT [FK_VendorsRegistrants_Vendor]
GO

/* Ratings Table */

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ratings]') AND type in (N'U'))
DROP TABLE [dbo].[Ratings]
GO

CREATE TABLE [dbo].[Ratings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RatingGivenByUserId] [bigint] NOT NULL,
	[JobId] [bigint] NULL,
	[JobHireId] [int] NULL,
	[JobStarRating] [int] NULL,
	[CaregiverId] [bigint] NULL,
	[CaregiverStarRating] [int] NULL,
	[FacilityId] [bigint] NULL,
	[FacilityStarRating] [int] NULL,
	[JobManagerId] [bigint] NULL,
	[JobManagerStarRating] [int] NULL,
	[VendorId] [int] NULL,
	[VendorStarRating] [int] NULL,
	[CreatedAt] [datetime2](7) NOT NULL DEFAULT (getdate()),
	[UpdatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Ratings] WITH CHECK ADD CONSTRAINT [FK_Ratings_Jobs] FOREIGN KEY([JobId])
REFERENCES [dbo].[Jobs] ([Id])
GO

ALTER TABLE [dbo].[Ratings] WITH CHECK ADD CONSTRAINT [FK_Ratings_JobHires] FOREIGN KEY([JobHireId]) 
REFERENCES [dbo].[JobHires] ([Id])
GO

ALTER TABLE [dbo].[Ratings] WITH CHECK ADD CONSTRAINT [FK_Ratings_Caregivers] FOREIGN KEY([CaregiverId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Ratings] WITH CHECK ADD CONSTRAINT [FK_Ratings_Facilities] FOREIGN KEY([FacilityId])
REFERENCES [dbo].[Companies] ([Id])
GO

ALTER TABLE [dbo].[Ratings] WITH CHECK ADD CONSTRAINT [FK_Ratings_JobManagers] FOREIGN KEY([JobManagerId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Ratings] WITH CHECK ADD CONSTRAINT [FK_Ratings_Vendors] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendors] ([Id])
GO

ALTER TABLE [dbo].[Ratings] WITH CHECK ADD CONSTRAINT [FK_Ratings_RatingGivenByUser] FOREIGN KEY([RatingGivenByUserId])
REFERENCES [dbo].[Users] ([Id])
GO