---
title: Milestone 3 API's
status: Completed
saif-comment: This is out of date and the table names are not correct, but Abdo implemented it.
Abdo status: Completed
Akshay status: Pending Testing
---

# Milestone 3 API's

## Job Module

### Tables (JobHires, TimeSheet, TimeSheetLocation)

```sql
-- JobHires, If job manager hires a caregiver, a row is created in this table.
-- Rules:
-- 1. A caregiver can only have one hired job per job.
CREATE TABLE JobHires (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    JobId BIGINT NOT NULL,
    FreelancerId BIGINT NOT NULL,
    HiringManagerId BIGINT NOT NULL, -- Manager Id who hired the caregiver
    Status VARCHAR(50) NOT NULL, -- Hired, Completed, Cancelled, Disputed
    Notes TEXT NULL,
    JobStartDate DATETIME NULL,
    JobEndDate DATETIME NULL,
    FOREIGN KEY (JobId) REFERENCES Job(Id),
    FOREIGN KEY (FreelancerId) REFERENCES Users(Id),
    FOREIGN KEY (HiredManagerId) REFERENCES Users(Id),
    FOREIGN KEY (PaymentMethodId) REFERENCES PaymentMethods(Id)
);

-- TimeSheet, If caregiver is hired, And caregiver clocks in, a row is created in this table.
-- Rules:
-- 1. A caregiver can only have one active timesheet at a time.
-- 2. A caregiver can have multiple timesheet per job.
-- 3. Notifications should be sent to the manager if the caregiver has clocked in or out.
CREATE TABLE TimeSheet (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    JobHireId BIGINT NOT NULL,
    UserId BIGINT NOT NULL,
    ClockIn DATETIME NOT NULL,
    ClockOut DATETIME NULL,
    TimeSheetNotes TEXT NULL,
    Status VARCHAR(50) NOT NULL, -- Active, Inactive, Completed, Cancelled, Disputed
    TimeSheetApprovalStatus VARCHAR(50) NULL, -- Pending, Approved, Rejected
    FOREIGN KEY (JobHireId) REFERENCES JobHires(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- TimeSheetLocation, If caregiver clocks in, or clocks out, a row is created in this table.
-- Rules:
-- 1. A timesheet location row is created every minute while clocked in.
-- 2. Should not create a row if the caregiver is not clocked in.
-- 3. Should not create a row if the caregiver is clocked out.
CREATE TABLE TimeSheetLocation (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    TimeSheetId BIGINT NOT NULL,
    LocationCapturedDateTime DATETIME NOT NULL,
    Location GEOGRAPHY NOT NULL,
    FOREIGN KEY (TimeSheetId) REFERENCES TimeSheet(Id)
);
```

### API's

- JobHires
  - [x] HireByJobId (POST) (/api/hiring)
    - { jobId: number, freelancerId: number, hiredManagerId: number }
    - Returns HiredJobId (number)
    - Action: 
      - 1. Check if freelancer is already hired for the same job. If yes, return error.
      - 2. Set HiredJob.Status to Hired
      - 3. Send notification to the manager and caregiver
  - [x] UpdateHiredJobStatus (POST) `/api/hiring-status`
    - { hiredJobId: number, status: string, notes: string }
    - Returns HiredJobId (number)
    - Action:
      - 1. Set HiredJob.Status to the new status
      - 2. Set HiredJob.Notes to the new notes
      - 3. Send notification to the manager and caregiver
  - [x] GetHiredJobById (GET) (/api/hiring/{id})
    - { hiredJobId: number }
    - Returns HiredJob (object)
    - Action:
      - 1. Get HiredJob by Id
  - [x] GetHiredJobByJobId (GET) (/api/hiring/job/{jobId})
    - { jobId: number }
    - Returns List of HiredJob (array of objects)
    - Action:
      - 1. Get list of HiredJob by JobId
  - [x] GetHiredJobByFreelancerId (GET) `/api/hiring/freelancer`
    - { accessToken: string }
    - Returns List of HiredJob (array of objects)
    - Action:
      - 1. Get list of HiredJob by FreelancerId
  - [x] GetHiredJobByManagerId (GET) `/api/hiring/manager`
    - { AccessToken: string }
    - Returns List of HiredJob (array of objects)
    - Action:
      - 1. Get list of HiredJob by ManagerId
- TimeSheet
  - [x] ClockinOnHiredJob (POST) `/api/clockin`
    - { hiredJobId: number, accessToken: string, notes: string, location: {GeographyObject} }
    - Returns TimeSheetId (number)
    - Action:
      - 1. Check if the timesheet is already clocked in. If yes, return error.
      - 2. Set TimeSheet.Status to Active
      - 3. Create TimeSheetLocation row
      - 4. Send notification to the manager
      - 5. Create row for location history with ClockIn
      - 6. Return TimeSheet Object
  - [x] ClockoutFromHiredJob (POST) `/api/clockOut`
    - { hiredJobId: number, accessToken: string, notes: string, location: {GeographyObject} }
    - Returns TimeSheetId (number)
    - Action:
      - 1. Check if the timesheet is already clocked out. If yes, return error.
      - 2. Set TimeSheet.Status to Completed
      - 3. Create TimeSheetLocation row
      - 4. Return TimeSheet Object
  - [x] GetTimeSheetById (GET) `/api/time-sheet/{id}`
    - { timeSheetId: number }
    - Returns TimeSheet (object)
    - Action:
      - 1. Get TimeSheet by Id
      - 2. Return TimeSheet Object
  - [x] GetTimeSheetByHiredJobId (GET) `/api/time-sheet/hired/{id}`
    - { hiredJobId: number }
    - Returns List of TimeSheet (array of objects)
    - Action:
      - 1. Get list of TimeSheet by HiredJobId
      - 2. Return List of TimeSheet
  - [x] GetTimeSheetByUserId (GET) `/api/time-sheet/user`
    - { accessToken: string }
    - Returns List of TimeSheet (array of objects)
    - Action:
      - 1. Get list of TimeSheet by UserId
      - 2. Return List of TimeSheet
  - [ ] GetTimeSheetLocationHistoryByTimeSheetId (GET) 
    - { timeSheetId: number }
    - Returns List of TimeSheetLocation (array of objects)
    - Action:
      - 1. Get list of TimeSheetLocation by TimeSheetId
      - 2. Return List of TimeSheetLocation
  - [ ] GetTimeSheetLocationHistoryByHiredJobId (GET)
    - { hiredJobId: number }
    - Returns List of TimeSheetLocation (array of objects)
    - Action:
      - 1. Get list of TimeSheetLocation by HiredJobId
      - 2. Return List of TimeSheetLocation
  - [ ] GetTimeSheetLocationHistoryByUserId (GET)
    - { accessToken: string }
    - Returns List of TimeSheetLocation (array of objects)
    - Action:
      - 1. Get list of TimeSheetLocation by UserId
      - 2. Return List of TimeSheetLocation
  - [ ] AddTimeSheetLocation (POST)
    - { timeSheetId: number, location: {GeographyObject} }
    - Returns TimeSheetLocationId (number)
    - Action:
      - 1. Create TimeSheetLocation row
      - 2. Return TimeSheetLocationId
