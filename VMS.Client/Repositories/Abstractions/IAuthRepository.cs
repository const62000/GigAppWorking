using GIgApp.Contracts.Requests.Login;
using GIgApp.Contracts.Responses;
using GIgApp.Contracts.Requests.Signup;
using GigApp.Domain.Entities;

namespace VMS.Client.Repositories.Abstractions;

public interface IAuthRepository
{
    Task<(string, string)> Login(LoginRequest request);
    Task<(SignupResult, string)> Signup(SignupRequest request);
    Task<User?> GetCurrentUser();
}