using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
namespace Connect.Application.Settings
{
    public class MailingService : IMailingService
    {
       
            private readonly MailSettings _mailSettings;

            public MailingService(IOptions<MailSettings> mailSettings)
            {
                _mailSettings = mailSettings.Value;
            }


        public async Task SendMailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = subject,
                From = { new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email) }
            };

            email.To.Add(MailboxAddress.Parse(mailTo));

            var builder = new BodyBuilder { HtmlBody = body };

            if (attachments != null)
            {
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await file.CopyToAsync(ms).ConfigureAwait(false);
                            var fileBytes = ms.ToArray();
                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }
            }

            email.Body = builder.ToMessageBody();

            using (var smtp = new SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls).ConfigureAwait(false);
                    await smtp.AuthenticateAsync(_mailSettings.Email, _mailSettings.Password).ConfigureAwait(false);
                    await smtp.SendAsync(email).ConfigureAwait(false);
                    await smtp.DisconnectAsync(true).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Failed to send email.", ex);
                }
            }
        }

    }
}
 