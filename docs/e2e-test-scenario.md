# End-to-End Test Scenarios

## Overview

This document outlines the end-to-end test scenarios for the Karegiver application. The test scenarios are designed to be run in a specific order to ensure that the application is working as expected.

The test procedures can be executed manually, or automatically via .NET integration tests and/or postman integration tests.

1. Test Freelancer Setup

   - A test freelancer caregiver named **TestFreelancer** is located at **TestFreelancerLocation**
   - Willing to travel up to 25 miles for a job (**TestJob1** will work for them but not **TestJob2**)

2. Test Hospital Network Setup

   - A fictional hospital network called **TestHospitalNetwork** exists in the Client table
   - This hospital network is a client of **Karegiver**
   - It has two facilities in the Client Locations table:
     - **TestFacility** (25 miles from **TestFreelancerLocation**, location of **TestJob1**)
     - **TestFacility2** (50 miles from **TestFreelancerLocation**, location of **TestJob2**)

3. Test Job Manager Setup

   - Each client has one assigned job manager
   - **TestJobManager** is assigned to **TestHospitalNetwork**
   - TestJobManager can create and manage jobs for both facilities:
     - **TestFacility** (**TestJob1**)
     - **TestFacility2** (**TestJob2**)

4. Job Creation Process
   - Job managers log in to create jobs
   - Jobs are named according to their facility:

| Test Object Type                    | Test Name      | 2nd Object Test Name |
| ----------------------------------- | -------------- | -------------------- |
| Test User                           | TestJobManager | TestJobManager2      |
| Managing Facility (Client Location) | TestFacility   | TestFacility2        |
| Job                                 | TestJob1       | TestJob2             |

The **TestFreelancerCaregiver** finds **TestJob1** in their search results but not **TestJob2** due to distance.

The **TestFreelancerCaregiver** creates a Stripe account as part of the onboarding process.

### Technical Requirements for .NET Integration Tests

These scenarios will be implented in [Test/GigApp.API.Tests/E2ETests.cs](../Test/GigApp.API.Tests/E2ETests.cs)

#### Testing Naming Conventions

The test must give names appropriately. For example, "TestHospitalNetwork" and "TestFacility" and for the second network and second facility, "TestHospitalNetwork2" and "TestFacility2" (establishing the naming convention that if the object or concept is singular, it does not need a number and anything following that requires a number. This naming convention is used throughout the test and the code.

### Karegiver Admin Setup

- [ ] An admin user is created in Auth0. This is done only once. Staging environment admin:
   - "password" : "P@ssword123",
   - "email": "Admin@gmail.com" 

#### Admin Actions in [VMS](https://vms.runasp.net)
- [ ] **KaregiverAdmin** creates a **TestHospitalNetwork** 
  - [ ] Navigate to "Clients" (a hospital is a client in the GigApp (Karegiver)) and create a new client.
  - [x] Click Add new
  - [x] Name it "TestHospitalNetwork"
  - [ ] Leave Address2 blank
    - [ ] BUG @abdo - Address2 is required.
    - [ ] Pending VMS Website update by @Abdo - @Prabat ignore these steps for now.
      - [ ] Use Google Places API to find the location of the hospital.
      - [ ] Click Save
- [x] **KaregiverAdmin** creates **TestFacility** and **TestFacility2**
- [ ] **KaregiverAdmin** creates a **TestJobManager** user
- [ ] **KaregiverAdmin** assigns both facilities to **TestJobManager**

## Test Flow Order

### 1. **VMS** Testing ([vms.runasp.net](https://vms.runasp.net)) (Must complete first)

- [ ] **TestVendorManager** registers:
  - [ ] Personal information (name, email, phone, password)
  - [ ] Vendor information (name, address, services)
- [ ] **TestVendorManager** logs into **VMS**
  - [ ] Redirects to /staff after login
- [ ] **TestVendorStaff1** Johnson registers:
  - [ ] Full Name: "**TestVendorStaff1** Johnson"
  - [ ] Email: "testvendorstaff1.johnson@test.com"
  - [ ] Phone: "555-0001"
  - [ ] Professional credentials
  - [ ] License information
  - [ ] Background check consent
- [ ] **TestVendorManager** manages staff:
  - [ ] Views staff list with details
  - [ ] Verifies **TestVendorStaff1** Johnson appears in list
  - [ ] Reviews and approves **TestVendorStaff1** Johnson's credentials
- [ ] **TestVendorManager** views staff timesheets:
  - [ ] Views timesheet list
  - [ ] Verifies details (name, job, dates, hours, status)

### 2. **M1** App Features (Complete flow)

### Job Posting

- [ ] TestJobManager logs in
- [ ] TestJobManager adds payment method (credit card)
  - [ ] System saves last 4 digits for display
  - [ ] Card is verified for future payments
- [ ] TestJobManager creates TestJob1 and TestJob2:
  - [ ] Title: "Registered Nurse Needed"
  - [ ] Type: Pay-as-you-go
  - [ ] Payment processed via added credit card
  - [ ] Deposit amount calculated based on hourly rate and hours per week
- [ ] TestJobManager adds job questionnaire to both jobs with questions:
  - [ ] "Years of RN experience?"
  - [ ] "Are you certified in CPR?"

### 3. **M2** App Features (Complete flow)

### Caregiver Registration & Application

- [ ] Create TestFreelancerCaregiver with:
  - [ ] Personal details
  - [ ] License information
  - [ ] Resume upload
  - [ ] Complete Stripe account onboarding
- [ ] TestFreelancerCaregiver searches for jobs:
  - [ ] Location: TestFreelancerLocation
  - [ ] Distance: Within 25 miles (should see TestJob1 but not TestJob2)
  - [ ] Filters: StartDate, EndDate, required licenses
- [ ] TestFreelancerCaregiver applies to TestJob1
  - [ ] Submits questionnaire responses
  - [ ] Attaches required documents

### Job Manager Review

- [ ] TestJobManager receives application notification for TestJob1
- [ ] TestJobManager reviews:
  - [ ] Caregiver profile
  - [ ] Questionnaire responses
  - [ ] Documents
- [ ] TestJobManager accepts TestJob1 application
- [ ] System automatically withdraws TestFreelancerCaregiver's other overlapping applications
- [ ] System sends withdrawal notifications with reason

### 4. **M3** App Features (Complete flow)

### Job Execution

- [ ] TestFreelancerCaregiver arrives at TestFacility location (TestJob1)
- [ ] TestFreelancerCaregiver clocks in (location verified within 500m)
- [ ] System tracks location every minute while clocked in
- [ ] TestFreelancerCaregiver records job duties in TimeCardNotes
- [ ] TestFreelancerCaregiver clocks out (location verified within 500m)
- [ ] TestFreelancerCaregiver marks TestJob1 as complete
- [ ] System sets Job.MarkedCompletedBy and Job.MarkedCompletedDateTime

### Job Manager Verification

- [ ] TestJobManager receives notifications for TestJob1:
  - [ ] Clock in
  - [ ] Clock out
  - [ ] Job completion
- [ ] TestJobManager monitors:
  - [ ] Clock in/out times
  - [ ] Location history
  - [ ] TimeCard notes
- [ ] TestJobManager approves TestJob1 completion
- [ ] System sets Job.ApprovedCompletedBy and Job.ApprovedCompletedDateTime

### Payment Processing

- [ ] System verifies escrow funds for TestJob1
- [ ] System processes payment from escrow
- [ ] TestFreelancerCaregiver receives payment in Stripe account
- [ ] Both parties receive completion notifications

### Ratings

- [ ] TestJobManager rates TestFreelancerCaregiver for TestJob1:
  - [ ] 1-5 star rating
  - [ ] Optional written feedback
  - [ ] System saves to Ratings table
- [ ] TestFreelancerCaregiver rates TestFacility:
  - [ ] 1-5 star rating
  - [ ] Optional written feedback
  - [ ] System saves to Ratings table
- [ ] Both parties can view their ratings history

### VMS Testing

- [ ] TestVendorManager registers:
  - [ ] Personal information (name, email, phone, password)
  - [ ] Vendor information (name, address, services)
- [ ] TestVendorManager logs into VMS
  - [ ] Redirects to /staff after login
- [ ] TestVendorManager manages staff:
  - [ ] Views staff list with details
  - [ ] Registers new TestVendorStaff
  - [ ] Verifies staff appears in list
- [ ] TestVendorManager views staff timesheets:
  - [ ] Views timesheet list
  - [ ] Verifies details (name, job, dates, hours, status)

### 5. Error scenarios and edge cases

## Test Classes Structure

### Base Classes

- [ ] TestUser (interface)
  - [ ] email
  - [ ] password
  - [ ] authentication token

### User Types

- [ ] KaregiverAdmin
- [ ] TestJobManager
- [ ] TestFreelancerCaregiver
- [ ] TestVendorManager
- [ ] TestVendorStaff

### Resource Classes

- [ ] TestClient
- [ ] TestFacility
- [ ] TestJob1
- [ ] TestJob2
- [ ] TestApplication
- [ ] TestTimeCard
- [ ] TestRating
- [ ] TestPayment

## Test Execution Order

1. [ ] M1 Features (Complete flow)
2. [ ] M2 Features (Complete flow)
3. [ ] M3 Features (Complete flow)
4. [ ] Error scenarios and edge cases
