using Business.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Business.Service.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public void SendEmail(string email, string subject, string htmlMessage)
        {
            var mimeEmail = new MimeMessage();
            mimeEmail.From.Add(MailboxAddress.Parse(_config["EmailUserName"]));
            mimeEmail.To.Add(MailboxAddress.Parse(email));
            mimeEmail.Subject = subject;
            mimeEmail.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };
            using var smtp = new SmtpClient();
            smtp.Connect(_config["EmailHost"], 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config["EmailUserName"], _config["EmailPassword"]);
            smtp.Send(mimeEmail);
            smtp.Disconnect(true);
        }
    }
}
