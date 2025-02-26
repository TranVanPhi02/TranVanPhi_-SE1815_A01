using BusinessObjects;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Microsoft.Extensions.Logging;

namespace TranVanPhiMVC.Extensions
{
    public class Mail
    {
        public class MailSettings
        {
            public string Mail { get; set; }
            public string DisplayName { get; set; }
            public string Password { get; set; }
            public string Host { get; set; }
            public int Port { get; set; }
        }

        public interface IEmailSender
        {
            Task SendEmailAsync(string email, string subject, string htmlMessage);
        }

        public class SendMailService : IEmailSender
        {
            private readonly MailSettings mailSettings;
            private readonly ILogger<SendMailService> logger;

            public SendMailService(IOptions<MailSettings> _mailSettings, ILogger<SendMailService> _logger)
            {
                mailSettings = _mailSettings.Value;
                logger = _logger;
                logger.LogInformation("Create SendMailService");
            }

            public async Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                var message = new MimeMessage();
                message.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
                message.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
                message.To.Add(MailboxAddress.Parse(email));  // Email của Lecturer
                message.Subject = subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = htmlMessage;  // Nội dung email (HTML body)

                message.Body = builder.ToMessageBody();

                using var smtp = new MailKit.Net.Smtp.SmtpClient();

                try
                {
                    smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                    await smtp.SendAsync(message);
                    logger.LogInformation("Mail sent successfully to: " + email);
                }
                catch (Exception ex)
                {
                    System.IO.Directory.CreateDirectory("mailssave");
                    var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                    await message.WriteToAsync(emailsavefile);

                    logger.LogInformation("Error sending email, saved at - " + emailsavefile);
                    logger.LogError(ex.Message);
                }

                smtp.Disconnect(true);
            }
        }
    }
}
