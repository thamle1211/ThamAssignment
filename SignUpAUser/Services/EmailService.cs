using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignUpAUser.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendEmail(string activateLink, string email)
        {
            var apiKey = _configuration.GetSection("Sendgrid:ApiKey").Value;
            var client = new SendGridClient(apiKey);
            var from_email = new EmailAddress("13520767@gm.uit.edu.vn");
            var subject = "Sending with Twilio SendGrid is Fun";
            var to_email = new EmailAddress(email);
            var plainTextContent = $"Please verify your account. Click this link: <a href='{activateLink}'>{activateLink}</a>";
            var htmlContent = $"<strong>Please verify your account. Click this link: <a href='{activateLink}'>{activateLink}</a>";
            var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }
    }
}
