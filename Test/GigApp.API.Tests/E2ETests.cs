using GIgApp.Contracts.Requests.BankAccount;
using GIgApp.Contracts.Requests.JobQuestion;
using GIgApp.Contracts.Requests.Jobs;
using GIgApp.Contracts.Requests.Login;
using GIgApp.Contracts.Requests.Signup;
using GIgApp.Contracts.Responses;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GIgApp.Contracts.Shared;
using System.Text;

namespace GigApp.API.Tests;

public class TestJobManager : TestUser
{
    JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    public string FacilityId { get; set; }

    public TestJobManager(string email, string password) : base(email, password) { }

    public async Task<string> AddPaymentMethod(BankAccountRequet bankAccountRequet)
    {
        var response = await Client.PostAsJsonAsync("/api/bank-accounts", bankAccountRequet);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<BaseFailResult>();
            Assert.Fail($"Request failed with status code: {response.StatusCode}. Error message: {result.Error}");
            response.EnsureSuccessStatusCode();
            return result.Error;
        }
        else
        {
            var result = await response.Content.ReadFromJsonAsync<BaseResult>();
            return result.Message;
        }
    }

    public async Task<long> CreateJob(CreateJobRequest createJobRequest)
    {
        var response = await Client.PostAsJsonAsync("/api/createjob", createJobRequest);

        response.EnsureSuccessStatusCode();
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<BaseResult>();
            return JsonSerializer.Deserialize<CreateJobResult>(result.Data.ToString(), options).jobId;
        }
        else
        {
            var result = await response.Content.ReadFromJsonAsync<BaseFailResult>();
            Assert.Fail($"Request failed with status code: {response.StatusCode}. Error message: {result.Error}");
            response.EnsureSuccessStatusCode();
            return 0;
        }
    }

    public async Task AddJobQuestionnaire(CreateJobQuestionRequest createJobQuestionRequest)
    {
        var response = await Client.PostAsJsonAsync($"api/job-questions/create",
            createJobQuestionRequest);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<BaseFailResult>();
            Assert.Fail($"Request failed with status code: {response.StatusCode}. Error message: {result.Error}");
            response.EnsureSuccessStatusCode();
        }
    }

    public async Task AddJObApplcation(CreateJobApplicationRequest applicationRequest)
    {
        var response = await Client.PostAsJsonAsync($"/api/jobs/{applicationRequest.JobId}/applications",
            applicationRequest);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<BaseFailResult>();
            Assert.Fail($"Request failed with status code: {response.StatusCode}. Error message: {result.Error}");
            response.EnsureSuccessStatusCode();
        }
    }


    public async Task Hiring(HireRequest hireRequest)
    {
        var response = await Client.PostAsJsonAsync(EndPoints.Hiring,
            hireRequest);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<BaseFailResult>();
            Assert.Fail($"Request failed with status code: {response.StatusCode}. Error message: {result.Error}");
            response.EnsureSuccessStatusCode();
        }
    }
}

public class TestFacility
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}

public class TestJob
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}

[TestClass]
public class E2ETests
{
    private TestReport _testReport;

    [TestInitialize]
    public void Initialize()
    {
        _testReport = new TestReport();
    }

    private async Task CleanupTestData(TestCleanupData testData)
    {
        try
        {
            var superAdmin = new TestSuperAdmin();
            // Delete job
            var response = await superAdmin.Client.DeleteAsync($"/api/jobs/{testData.JobId}");
            if (!response.IsSuccessStatusCode)
            {
                _testReport.AddCleanupError($"Failed to delete job {testData.JobId}");
            }

            // Remove payment method
            response = await superAdmin.Client.DeleteAsync($"/api/bank-accounts/{testData.PaymentMethodId}");
            if (!response.IsSuccessStatusCode)
            {
                _testReport.AddCleanupError($"Failed to delete payment method {testData.PaymentMethodId}");
            }

            // Remove facility assignment
            response = await superAdmin.Client.DeleteAsync($"/api/facilities/{testData.FacilityId}/managers/{testData.JobManagerEmail}");
            if (!response.IsSuccessStatusCode)
            {
                _testReport.AddCleanupError($"Failed to remove facility assignment");
            }

            // Delete facility
            response = await superAdmin.Client.DeleteAsync($"/api/facilities/{testData.FacilityId}");
            if (!response.IsSuccessStatusCode)
            {
                _testReport.AddCleanupError($"Failed to delete facility {testData.FacilityId}");
            }

            // Delete job manager
            response = await superAdmin.Client.DeleteAsync($"/api/users/{testData.JobManagerEmail}");
            if (!response.IsSuccessStatusCode)
            {
                _testReport.AddCleanupError($"Failed to delete job manager {testData.JobManagerEmail}");
            }
        }
        catch (Exception ex)
        {
            _testReport.AddCleanupError($"Cleanup failed with exception: {ex.Message}");
        }
    }

    [TestMethod]
    public async Task GigApp_E2E_Test()
    {
        try
        {
            var testId = Guid.NewGuid().ToString();
            _testReport.StartNewTest("GigApp E2E Test", testId);

            // M1: Super Admin Setup
            _testReport.StartSection("M1 Features - Super Admin Setup");
            
            var superAdmin = new TestSuperAdmin();
            var loginResult = await superAdmin.Login();
            _testReport.AddStep("Super Admin Login", loginResult);

            // Create facility
            var facilityId = await superAdmin.CreateFacility(
                name: "Test Care Center" + testId,
                address: new AddressRequest("Dokki", "Faisl", "Giza", "Egypt", 20, 20,"123")
            );
            _testReport.AddStep("Create Test Facility", facilityId > 0);

            // Create job manager
            var jobManager = await superAdmin.CreateJobManager(
                new SignupRequest(
                    "manager-" + testId, 
                    "manager-" + testId, 
                    $"e2e-test-{testId}@test.com", 
                    "Manager123!",
                    new List<GIgApp.Contracts.Enums.UserType> { 
                        GIgApp.Contracts.Enums.UserType.JobProvider, 
                        GIgApp.Contracts.Enums.UserType.Freelancer 
                    }, 
                    null,
                    new List<LicenseRequest>()
                )
            );
            _testReport.AddStep("Create Job Manager", jobManager != null);

            // Assign facility to manager
            //var assignResult = await superAdmin.AssignFacilityToManager(
            //    managerId: jobManager.Email,
            //    facilityId: facilityId
            //);
            //_testReport.AddStep("Assign Facility to Manager", assignResult.Status);

            // M1: Job Posting
            var loginResult2 = await jobManager.Login();
            _testReport.AddStep("Job Manager Login", loginResult2);

            // Add payment method
            //var paymentMethodId = await jobManager.AddPaymentMethod(
            //    new BankAccountRequet(0, "Test Bank", "Test Account", "1234567890", "123", "Test Route", "Test Swift", "Test IBAN",string.Empty)
            //);
            //_testReport.AddStep("Add Payment Method", !string.IsNullOrEmpty(paymentMethodId));

            // Create job
            var jobId = await jobManager.CreateJob(
                new CreateJobRequest(
                    "Registered Nurse Needed",
                    "We are looking for an experienced RN",
                    "Valid RN License Required",
                    "2+ years experience required",
                    DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                    TimeOnly.FromDateTime(DateTime.Now.AddHours(9)),
                    50.00m,
                    "open",
                    4,
                    3,
                    0,
                    0,
                    1,
                    DateTime.Now,
                    DateTime.Now
                )
            );
            _testReport.AddStep("Create Job", jobId > 0);

            // Add job questionnaire
            await jobManager.AddJobQuestionnaire(
                new CreateJobQuestionRequest(
                    jobId,
                    "Years of RN experience?",
                    DateTime.Now
                )
            );

            await jobManager.AddJobQuestionnaire(
                new CreateJobQuestionRequest(
                    jobId,
                    "Are you certified in CPR?",
                    DateTime.Now
                )
            );

            // Store test data for cleanup
            TestData.Add(new TestCleanupData
            {
                FacilityId = facilityId,
                JobManagerEmail = jobManager.Email,
                JobId = jobId,
                PaymentMethodId = string.Empty,
            });

            // M2: Placeholder for Caregiver Registration & Application (Future Implementation)
            /* TODO: Implement when ready
            // Create and setup test caregiver
            var caregiver = await CreateTestCaregiver(testId);
            await caregiver.AddLicenseInfo(new LicenseRequest(...));
            await caregiver.AddBankAccount(new BankAccountRequest(...));
            
            // Search and apply for job
            var searchResults = await caregiver.SearchJobs(location: "New York", distance: 25);
            await caregiver.ApplyForJob(jobId, new QuestionnaireResponse(...));
            */

            // M3: Placeholder for Job Execution & Payment Processing (Future Implementation)
            /* TODO: Implement when ready
            // Job execution
            await caregiver.LogArrival(jobId);
            await caregiver.RecordDuties(jobId, new List<string> { "Administered medication", "Vital signs check" });
            await jobManager.MonitorPerformance(jobId);
            await jobManager.CompleteJob(jobId);

            // Payment processing
            var paymentStatus = await System.CheckPaymentStatus(jobId);
            Assert.AreEqual("Completed", paymentStatus);
            */

            _testReport.TestPassed = true;
        }
        catch (Exception ex)
        {
            _testReport.TestPassed = false;
            _testReport.AddError($"Test failed with exception: {ex.Message}");
            throw;
        }
        finally
        {
            // Generate the report
            await _testReport.SaveReportAsync();
        }
    }

    // Add cleanup method
    [TestCleanup]
    public async Task Cleanup()
    {
        foreach (var testData in TestData)
        {
            await CleanupTestData(testData);
        }
    }

    private static List<TestCleanupData> TestData = new();

    private class TestCleanupData
    {
        public long FacilityId { get; set; }
        public string JobManagerEmail { get; set; }
        public long JobId { get; set; }
        public string PaymentMethodId { get; set; }
    }
}

public class TestReport
{
    private List<TestSection> _sections = new();
    private List<string> _cleanupErrors = new();
    private List<string> _errors = new();
    public bool TestPassed { get; set; }
    public string TestId { get; private set; }
    public string TestName { get; private set; }

    public void StartNewTest(string name, string testId)
    {
        TestName = name;
        TestId = testId;
    }

    public void StartSection(string name)
    {
        _sections.Add(new TestSection { Name = name });
    }

    public void AddStep(string stepName, bool success)
    {
        if (_sections.Count == 0) StartSection("Uncategorized Steps");
        _sections[^1].Steps.Add(new TestStep 
        { 
            Name = stepName, 
            Success = success,
            Timestamp = DateTime.UtcNow
        });
    }

    public void AddError(string error)
    {
        _errors.Add(error);
    }

    public void AddCleanupError(string error)
    {
        _cleanupErrors.Add(error);
    }

    public async Task SaveReportAsync()
    {
        var report = new StringBuilder();
        report.AppendLine($"# E2E Test Report");
        report.AppendLine($"Test: {TestName}");
        report.AppendLine($"ID: {TestId}");
        report.AppendLine($"Date: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");
        report.AppendLine($"Status: {(TestPassed ? "✅ PASSED" : "❌ FAILED")}");
        report.AppendLine();

        foreach (var section in _sections)
        {
            report.AppendLine($"## {section.Name}");
            foreach (var step in section.Steps)
            {
                report.AppendLine($"- {(step.Success ? "✅" : "❌")} {step.Name} ({step.Timestamp:HH:mm:ss})");
            }
            report.AppendLine();
        }

        if (_errors.Any())
        {
            report.AppendLine("## Errors");
            foreach (var error in _errors)
            {
                report.AppendLine($"- ❌ {error}");
            }
            report.AppendLine();
        }

        if (_cleanupErrors.Any())
        {
            report.AppendLine("## Cleanup Errors");
            foreach (var error in _cleanupErrors)
            {
                report.AppendLine($"- ⚠️ {error}");
            }
        }

        // Save to file
        var fileName = $"e2e-test-report-{TestId}.md";
        await File.WriteAllTextAsync(fileName, report.ToString());
    }
}

public class TestSection
{
    public string Name { get; set; }
    public List<TestStep> Steps { get; set; } = new();
}

public class TestStep
{
    public string Name { get; set; }
    public bool Success { get; set; }
    public DateTime Timestamp { get; set; }
}