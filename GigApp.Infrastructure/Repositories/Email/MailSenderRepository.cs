using MimeKit;
using MailKit.Net.Smtp;
using System.Security.Authentication;
using GigApp.Application.Interfaces.Email;
using GigApp.Application.Options.Auth0;
using Microsoft.Extensions.Options;
using GigApp.Application.Options.MailServer;

namespace GigApp.Infrastructure.Repositories.Email
{
    public class MailSenderRepository(IOptions<MailServerOption> options) : IMailSenderRepository
    {
        public async Task<bool> SendMail(string email, string subject, string message)
        {
            await Task.CompletedTask;
            var mailOptions = options.Value;
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("YourAppName", mailOptions.From));
            mimeMessage.To.Add(new MailboxAddress("RecipientName", email));
            mimeMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = message };
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            //using (var client = new SmtpClient())
            //{
            //    client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Ssl2 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
            //    client.CheckCertificateRevocation = false;
            //    await client.ConnectAsync(mailOptions.SmtpServer, mailOptions.Port, true);
            //    await client.AuthenticateAsync(mailOptions.UserName, mailOptions.Password);
            //    await client.SendAsync(mimeMessage);
            //    await client.DisconnectAsync(true);
            //}
            return true;
        }
    }
}
