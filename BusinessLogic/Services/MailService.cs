using BusinessLogic.Interfaces;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using BusinessLogic.Helpers;

namespace BusinessLogic.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendMailAsync(string subject, string body, string toSend, string? fromSend = null)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(fromSend));
            email.To.Add(MailboxAddress.Parse(toSend));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = $"<h1>Your order</h1>{body}" };

            MailData Data = _configuration.GetSection("MailData").Get<MailData>()!;
            string EmailFrom = Data.EmailFrom;
            string Host = Data.Host;
            int Port = Data.Port;
            string Password = Data.Password;


            using var smtp = new SmtpClient();
            smtp.Connect(Host, Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(EmailFrom, Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
