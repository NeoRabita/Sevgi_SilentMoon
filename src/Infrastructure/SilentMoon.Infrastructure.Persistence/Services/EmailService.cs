using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SilentMoon.Application.DTOs.Email;
using SilentMoon.Application.Interfaces.Services;
using SilentMoon.Infrastructure.Persistence.Settings;

namespace SilentMoon.Infrastructure.Persistence.Services
{
    public class EmailService : IEmailService
    {
        public MailSettings _apiSettings;

        public EmailService(IOptions<MailSettings> apiSettings)
        {
            _apiSettings = apiSettings.Value;
        }

        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(request.From ?? _apiSettings.EmailFrom, _apiSettings.DisplayName);
                mail.To.Add(request.To);
                mail.Subject = request.Subject;
                var htmlView = AlternateView.CreateAlternateViewFromString(request.Body, null, "text/html");
                mail.IsBodyHtml = true;
                mail.AlternateViews.Add(htmlView);
                using (var smtpClient = new SmtpClient(_apiSettings.SmtpHost, _apiSettings.SmtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_apiSettings.SmtpUser, _apiSettings.SmtpPass);
                    smtpClient.EnableSsl = _apiSettings.SSL;
                    await smtpClient.SendMailAsync(mail);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}