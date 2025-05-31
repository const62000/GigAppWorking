namespace GigApp.Application.Interfaces.Email
{
    public interface IMailSenderRepository
    {
        Task<bool> SendMail(string email,string subject,string message);
    }
}
