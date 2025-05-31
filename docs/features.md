# Features and User Stories

- **Updated**: Jan 31, 2025

## Introduction

This document outlines the features and user stories for the Karegiver App and the VMS. Some User Stories have more details than others as needed.

### Definitions

- **Vendor Management System (VMS)**: a web app for the vendor portal.
  - Staging Environment: https://vms.runasp.net
  - Production Environment: TBD

### User Roles

- **Administrator**: Has full access to the system.
- **Vendor Manager**: Has access to the vendor portal, can create and manage vendors, and manage staff.
- **Job Manager**: Can create and manage jobs, and manage caregivers who have been hired by the vendor.
- **Caregiver**: Has access to the caregiver mobile app, can apply to jobs, and clock in/out.
  - Sub-types:
    - **Freelancer Caregiver**: Is not associated with a vendor.
    - **Vendor Staff**: Is associated with a vendor.

A user can have multiple roles. For example, a vendor manager can also be a job manager.

**Access to Vendor Portal**

- Only Administrators and Vendor Managers can access the VMS.

## User Management Functionality

### Feature 1. User Management

- [x] **User Story 1.1 (M1)**: As an **Administrator**, I want to create vendors and manage their basic information.
  - **Acceptance Criteria**:
    - [x] On the `/vendors` page, if the user has the Administrator role, an "Add Vendor" button is displayed.
    - [x] Clicking the "Add Vendor" button allows the administrator to create a new vendor.
    - [x] On the `/vendors` page, if the user has the Administrator role, they are allowed to view, edit, and delete existing vendors.
    - [x] The system allows the administrator to update the basic information of an existing vendor.
    - [x] The system allows the administrator to delete a vendor.
- [2] **User Story 1.2 (M1)**: As an **Administrator**, I want to register vendor managers and assign them to specific vendors.
  - **Acceptance Criteria**:
    - [x] On the `/vendors` page, if the user has the Administrator role, a "Register Vendor Manager" button is displayed in the Vendor Manager column if one has not been assigned. Otherwise, their name appears.
    - [x] Clicking the "Register Vendor Manager" button allows the administrator to register a new vendor manager.
- [3] **User Story 1.3 (M1)**: As an **Administrator**, I want to register vendor staff members and associate them with their respective vendors.
  - **Acceptance Criteria**:
    - [x] On the `/staff` page, if the user has the Administrator role, an "Add Staff" button is displayed.
    - [x] Clicking the "Add Staff" button allows the administrator to create a new vendor staff member.
    - [x] On the `/staff` page, if the user has the Administrator role, they are allowed to view, edit, and delete existing vendor staff members.
    - [x] The system allows the administrator to associate a vendor staff member to a specific vendor.
    - [x] The system allows the administrator to associate a vendor staff member to a specific vendor.

## Primary Functionality

### Feature 2. Facility (Client Location) Registration

- [x] **User Story 2.1 (M1)**: As a **Administrator**, I want to register companies and facilities.
  - **Acceptance Criteria**:
    - [x] If the user has the Administrator role, "Clients & Locations" is displayed in the navigation bar.
    - [x] Clicking "Clients & Locations" navigates to the `/clients` page.
    - [x] The `/clients` page should have authorization logic in it (not just in the Navigation component).
    - [x] Clicking the "Add Client" button allows the administrator to create a new client.
    - [x] On the `/clients` page, if the user has the Administrator role, they are allowed to view, edit, and delete existing clients.
    - [x] When the user clicks "View" on a client, they are redirected to the `/clients/{clientId}` page.
    - [x] The `/clients/{clientId}` lists facilities associated with the client and provies CRUD capabilities.
    - [x] The `/clients/{clientId}` page a list of Job Managers associated with the client.
    - [x] The `/clients/{clientId}` page allows the administrator to add a new Job Manager.
    - [ ] The `/clients/{clientId}` page allows the administrator to edit a Job Manager.
    - [x] The `/clients/{clientId}` page allows the administrator to delete a Job Manager.

### Feature 3. Job Manager Registration

- [x] **User Story 3.1 (M1)**: As a **Job Manager**, I want to be able to register and assign facilities to users, which will automatically make them Job Managers.
  - **Update note**: This was simplified from earlier version which required managing a standalone "Job Manager" role. With this approach, a user is allowed to post jobs by being a registered client user which is associated with a facility.

### Feature 4. Job Posting

[Feature 4 Demo](https://www.loom.com/share/294db6a105364869ba8515d993a7d280?sid=19d71d8e-9dd1-46ee-b87c-916b6fb5b345)

- [x] **User Story 4.1 (M1)**: As a Job Manager, I want to log into the Caregiver mobile app, navigate to the job posting section, enter job details (title, description, requirements, start date, end date, location, rates, etc.), select "Contract" or "Pay as you Go." If "Pay as you Go" is selected, a payment should be collected.
  - **Acceptance Criteria**
    - [x] If user or company has one or more credit cards on file, the last 4 digits of the cards will be displayed in a dropdown menu.
    - [x] An "Add Card" payment method button will always be displayed in this section.
    - [x] Add Card goes to a second page with a form to fill out credit card information.
    - [x] On successful submission, the system will save the card and the last 4 digits will be displayed in the dropdown menu.
    - [x] If "Contract" is selected: "Coming soon in M2-M3" will be displayed.
- [x] **User Story 4.2 (M1)**: As a Job Manager, I want to optionally add a Job Questionnaire to the job posting.

### Feature 5. Payment Processing

[M3-focused Demo (Job posting and application, clocking in/out, job completion, payment processing, job search and filtering, adding notes to time cards)](https://www.loom.com/share/bc92f5efde3d4eef8f83267834817ff8)

- [x] **User Story 5.1 (M3)**: As a Job Manager, I want to proceed to select payment type (contract or pay as you go).
- [x] **User Story 5.2 (M3)**: As a Job Manager, I want to enter credit or debit card details to pay escrow funds for the job post.
- [x] **User Story 5.3 (M3)**: System will process the payment. If the payment fails, it will prompt the Job Manager to re-enter payment. If the payment succeeds, it will notify the Job Manager and hold the funds in escrow.
  - **Acceptance Criteria**:
    - Payment is processed when the Job Manager approves the job completion.
    - The system verifies that `Job.ApprovedCompletedBy` is set.
    - Upon processing the payment, the system notifies the Job Manager and the Caregiver of the successful transaction.
  - **Non-functional Requirements**:
    - The system must prevent payment failures by ensuring that a deposit is taken in advance.
    - `Job.DepositAmount` should always be higher than the `MaxWeeklyPayment`, calculated as `Job.HourlyRate * Job.HoursPerWeek`.
    - The system must securely hold funds in escrow until job completion is verified.
- [x] **User Story 5.4 (M3)**: System will confirm and save payment information.

### Feature 6. Caregiver Registration

[Feature 6 Demo](https://www.loom.com/share/f338c6e0f0f54249b7f902cac74c3d1a?sid=70e161bc-6ea3-41f1-902b-1199683b62d1)

- [x] **User Story 6.1 (M2)**: As a **Freelancer Caregiver**, I want to open the Karegiver mobile app, select the option to register as a caregiver, enter personal details, contact information, and license information, and submit my registration.

  - **Acceptance Criteria**:
    - [ ] Caregiver can upload a resume or license information.
    - [ ] Caregiver can login to the Karegiver mobile app after registration.

- [x] **User Story 6.2 (M2)**: As a **Freelancer Caregiver**, I want to provide my stripe account information for payments.
  - **Acceptance Criteria**:
    - [ ] During the registration process, the caregiver will be redirected to the Stripe onboarding page.
    - [ ] After the caregiver completes the onboarding process, the mobile app will be updated with the caregiver's Stripe account information.
    - [ ] The database will be updated with the caregiver's Stripe account information.
- [ ] **User Story 6.3 (M2)**: As a **Vendor Staff Caregiver**, I want to open the Karegiver mobile app, select the option to regisdter as a caregiver, enter personal details, contact information, and license information, and submit my registration.

### Feature 7. Caregiver Job Search and Application

[Feature 7 Demo](https://www.loom.com/share/1aca036ddfa344548d8fb3af40853c03?sid=4ffe1e39-d6dc-4711-a3c2-475563baa43b)

- [x] **User Story 7.1 (M2)**: As a Caregiver, I want to log into the Karegiver mobile app, use the search feature to find jobs and filter job searches by job requirements, required licenses, city, and country.
  - **Acceptance Criteria**:
    - [x] Caregiver can search for jobs by job requirements, StartDate, EndDate, required licenses, city, and country.
- [x] **User Story 7.2 (M3)**: As a Caregiver, I want to filter job searches by my distance to location by entering my location and specifying the maximum distance in miles that the facility offering the job should be.
  - [M3-focused Demo (Job posting and application, clocking in/out, job completion, payment processing, job search and filtering, adding notes to time cards)](https://www.loom.com/share/bc92f5efde3d4eef8f83267834817ff8)
- [x] **User Story 7.3 (M2)**: As a Caregiver, I want to submit applications and answer Job Questionnaires.
  - **Acceptance Criteria**:
    - [ ] System will allow a caregiver to apply to a job if they have already applied to another job overlapping time in the StartDate and EndDate.

### Feature 8. Job Application Acceptance

[Feature 8 Demo](https://www.loom.com/share/bf4cbe3dc9d44518b5df238772e25c10?sid=b53a0020-b883-433e-b7a7-72b07d48b59f)

- [x] **User Story 8.1 (M2)**: As a Job Manager, I want to receive notification of caregiver applications.
- [x] **User Story 8.2 (M2)**: As a Job Manager, I want to review Caregiver applications, profiles and Job Questionnaire responses.
- [x] **User Story 8.3 (M2)**: As a Job Manager, I want to accept or reject applications from Caregivers.
  - **Acceptance Criteria**:
    - [ ] **Backend**: Upon accepting an application, the system will withdraw all other applications that the caregiver has submitted for any jobs which overlap in time with the job they were accepted for.
    - [ ] **Backend**: When withdrawing an application, the system will notify the caregiver of the withdrawal with a reason of "This application was withdrawn because another application was accepted which overlaps with this application."
- [M3-focused Demo (Job posting and application, clocking in/out, job completion, payment processing, job search and filtering, adding notes to time cards)](https://www.loom.com/share/bc92f5efde3d4eef8f83267834817ff8)
  - [x] **User Story 8.4 (M3)**: As a Job Manager, I want to prevent Caregivers from double-booking themselves.
  - [x] **User Story 8.5 (M3)**: As a Facility/Job Manager, I want to put a "Approved" for a job, they can assign themself a shift and clock in/out.

### Feature 9. Job Performance and Completion

[M3-focused Demo (Job posting and application, clocking in/out, job completion, payment processing, job search and filtering, adding notes to time cards)](https://www.loom.com/share/bc92f5efde3d4eef8f83267834817ff8)

- [x] **User Story 9.1 (M3)**: As a Caregiver, I want to log my arrival at the job location at the scheduled time.
  - **Acceptance Criteria**:
    - The system shall capture the caregiver's current location when they clock in.
    - A caregiver can clock in only when at the job location or within an acceptable radius (e.g., 500 meters).
    - A new entry is created in the `TimeCard` table with `ClockIn` time when the caregiver clocks in.
    - The system stores the caregiver's location history while they are clocked in.
  - **Non-functional Requirements**:
    - The system shall record the caregiver's location every minute while they are clocked in.
    - Location tracking shall stop when the caregiver clocks out
- [x] **User Story 9.2 (M3)**: As a Caregiver, I want to log performing the duties as described in the job posting.
  - **Acceptance Criteria**:
    - The caregiver can enter notes about the work performed in the `TimeCardNotes` field.
    - The system allows the caregiver to update `TimeCardNotes` while clocked in.
- [x] **User Story 9.3 (M3)**: As a Caregiver, I want to log completing the duties as described in the job posting.
  - **Acceptance Criteria**:
    - The system shall capture the caregiver's current location when they clock out.
    - A caregiver can clock out only when at the job location or within an acceptable radius.
    - The caregiver can mark the job as completed by clicking a "Complete Job" button, visible only to them.
    - Upon job completion, the system sets `Job.MarkedCompletedBy` to the caregiver's `Id` and `Job.MarkedCompletedDateTime` to the current date and time.
    - Notifications are sent to the Job Manager when:
      - The caregiver clocks in.
      - The caregiver clocks out.
      - The caregiver marks the job as completed.
- [x] **User Story 9.4 (M3)**: As a Job Manager, I want to monitor and verify the job performance.
  - **Acceptance Criteria**:
    - The Job Manager can view the caregiver's clock-in and clock-out times.
    - The Job Manager can view the caregiver's location history during the job.
    - The Job Manager receives notifications when the caregiver clocks in, clocks out, and completes the job.
- [x] **User Story 9.5 (M3)**: As a Job Manager, I want to mark the job as complete, upon which payment will be paid from the credit card and deposited into the Caregiver's Stripe account.
  - **Acceptance Criteria**:
    - After the caregiver marks the job as completed, the Job Manager can review and approve the completion.
    - The Job Manager clicks "Approve Completion" to confirm the job is completed satisfactorily.
    - Upon approval, the system sets `Job.ApprovedCompletedBy` to the Job Manager's `Id` and `Job.ApprovedCompletedDateTime` to the current date and time.
    - The system processes the payment to the caregiver's bank account.
  - **Non-functional Requirements**:
    - Payment processing should be secure and comply with relevant financial regulations.
    - The system should ensure that sufficient funds are available in the escrow account before releasing payment.

### Feature 10. Fund Release

[M3-focused Demo (Job posting and application, clocking in/out, job completion, payment processing, job search and filtering, adding notes to time cards)](https://www.loom.com/share/bc92f5efde3d4eef8f83267834817ff8)

- [ ] **User Story 10.1 (M3)**: As a Job Manager and Caregiver, I want to be notified of successful transactions.
  - **Acceptance Criteria**:
    - [ ] Once a payment is processed, the system should notify the Job Manager and the Caregiver of the successful transaction.
      - [x] Development
      - [x] Testing
      - [ ] Content Validation

### Feature 11. Ratings

- [ ] **Technical Enabler 11.A (M3)**: Select a flutter rating component.

#### Caregiver Ratings

- [ ] **User Story 11.1 (M3)**: As a Job Manager, I want to rate the Caregiver's performance after job completion.

  - **Acceptance Criteria**:
    - [x] The Job Manager can rate the Caregiver on a 1-5 star scale
    - [x] API endpoint: `POST /api/caregivers/{caregiverId}/ratings/{jobId}`
    - [x] API receives `JobId`, `JobHireId`, `CaregiverId`, `CaregiverStarRating` and saves it to the `Ratings` table
    - [x] The rating can only be submitted after job completion
    - [x] The Job Manager can optionally provide written feedback
    - [x] The rating must be visible after submission.
      - [x] API Endpoint: `GET /api/caregivers/{caregiverId}/ratings/{jobId}`

- [ ] **User Story 11.2 (M3)**: As a Job Manager, I want to view all ratings given to a Caregiver.
  - **Acceptance Criteria**:
    - [x] Job Manager can view a list of all ratings for a specific Caregiver
    - [x] API endpoint: `GET /api/caregivers/{caregiverId}/ratings`
    - [x] The list shows rating date, star rating, and feedback for each rating
    - [x] Ratings are sorted by date with most recent first
    - [x] The average rating is displayed prominently

#### Facility Ratings

- [ ] **User Story 11.3 (M3)**: As a Caregiver, I want to rate the Facility (ClientLocation) after completing a job there.

  - **Acceptance Criteria**:
    - [x] The Caregiver can rate the Facility on a 1-5 star scale
    - [ ] API endpoint: `POST /api/addresses/{addressId}/ratings`
      - [ ] Frontend get from `JobHire.Job.AddressId`
    - [x] API receives `JobId`, `JobHireId`, `FacilityId`, `FacilityStarRating` and saves it to the `Ratings` table
    - [x] The rating can only be submitted after job completion
    - [x] The Caregiver can optionally provide written feedback
    - [x] The rating must be visible after submission.
      - [ ] API Endpoint: `GET /api/addresses/{addressId}/ratings`

- [ ] **User Story 11.4 (M3)**: As a Caregiver, I want to view all ratings given to a Facility.
  - **Acceptance Criteria**:
    - [x] Caregiver can view a list of all ratings for a specific Facility
    - [x] API endpoint: `GET /api/facilities/{facilityId}/ratings`
    - [x] The list shows rating date, star rating, and feedback for each rating
    - [x] Ratings are sorted by date with most recent first
    - [x] The average rating is displayed prominently

## Vendor Management System (VMS)

### Feature 12. VMS Login and Registration

- [x] **User Story 12.1 (M3)**: As a Vendor Manager, I want to navigate to the home page, click on the "Register" link, and **register the vendor and myself as a vendor manager**.

  - **Acceptance Criteria**:
    - [x] When the user registers the vendor and themself, they are assigned as the Vendor Manager.
    - [x] Only one vendor manager is allowed per vendor.
    - [x] SQL Script updated
    - [x] All the registration fields should be on one page and should be separated into sections as such:
      - **User Information**: First Name, Last Name, Email, Phone Number, Password
      - **Vendor Information**: Vendor Name, Vendor Address, Vendor City, Vendor State, Vendor Zip, Services Offered

- [x] **User Story 12.2 (M3)**: As a Vendor Manager, I want to login to the vendor portal.

  - **Acceptance Criteria**:
    - [ ] The home page should display a "Login" link
    - [ ] Clicking "Login" navigates to '/login'
    - [ ] After successful login, user is redirected to '/staff'
    - [ ] There should be a "Logout" button which logs the user out and redirects to the login page.

### Feature 13. VMS Staff Management

- [x] **User Story 13.1 (M3)**: As a Vendor Manager, I want to view my staff members.

  - **Acceptance Criteria**:
    - [ ] The '/staff' page displays a list of all staff members associated with my vendor account
    - [ ] Staff list shows relevant details like name, role, and status

- [x] **User Story 13.2 (M3)**: As a Vendor Manager, I want to register new staff members.

  - **Acceptance Criteria**:
    - [ ] Registration form captures all required staff information
    - [ ] Successful submission creates new staff record in database
    - [ ] User is redirected to '/staff' page after successful submission
    - [ ] New staff member appears in the staff list immediately

### Feature 14. VMS Timesheet Management

- [ ] **User Story 14.1 (M3)**: As a Vendor Manager, I want to view all timesheets for my staff.
  - **Acceptance Criteria**:
    - [ ] The '/timesheets' page displays a list of all timesheets for my staff
    - [ ] Timesheets list shows relevant details like staff name, job title, start date, end date, hours worked, timesheet approval status, and payment status
