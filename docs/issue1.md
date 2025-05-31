---
title: Getting only 1 freelancer that was hired for the job
Abdo status: Not Started
Akshay status: Not Tested
---

# Description
Getting only 1 freelancer in the response for 


# Steps to Reproduce
1. Call https://gig-api-stage.azurewebsites.net/api/job-questions/by-job/226
2. Check the response

# Expected Behavior
Should get all freelancers that were hired for the job

# Actual Behavior
Getting only 1 freelancer that was hired for the job
```json
{data: {id: 0, jobId: 0, freelancerId: 0, hiredManagerId: 0, status: null, note: 
null, startTime: 0001-01-01T00:00:00, endTime: 0001-01-01T00:00:00, createdAt:
2024-12-17T18:09:17.5112556+00:00, freelancer: null, hiredManager: null, job: null, timeSheets: []},
status: true, message: }
```

