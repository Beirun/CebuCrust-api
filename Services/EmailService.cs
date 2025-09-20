using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CebuCrust_api.Interfaces;
using DotNetEnv;

namespace CebuCrust_api.Services
{
    

    public class EmailService : IEmailService
    {
        private readonly string _email;
        private readonly string _password;


        public EmailService()
        {
            Env.Load();
            _email = Environment.GetEnvironmentVariable("APP_EMAIL")!;
            _password = Environment.GetEnvironmentVariable("APP_PASSWORD")!;
        }

        public async Task SendResetEmailAsync(string toEmail, string resetCode)
        {
            string resetLink = $"http://localhost:5173/reset/{resetCode}";
            string subject = "Password Reset Request";
            string body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                  <meta charset='UTF-8'>
                  <style>
                    body {{ font-family: Arial, sans-serif; background-color: #f6f6f6; padding: 20px; }}
                    .container {{ max-width: 600px; margin: auto; background: #ffffff; padding: 30px; border-radius: 8px; box-shadow: 0 2px 6px rgba(0,0,0,0.1); }}
                    h2 {{ color: #333333; margin-bottom: 20px; }}
                    p {{ color: #555555; line-height: 1.6; }}
                    .btn {{ display: inline-block; margin-top: 20px; padding: 12px 24px; background-color: #4CAF50; color: #ffffff; text-decoration: none; border-radius: 4px; font-weight: bold; }}
                    .footer {{ margin-top: 30px; font-size: 12px; color: #999999; }}
                  </style>
                </head>
                <body>
                  <div class='container'>
                    <h2>Password Reset Request</h2>
                    <p>Hello,</p>
                    <p>We received a request to reset your password. Click the button below to create a new one. This link will expire shortly for your security.</p>
                    <a class='btn' href='{resetLink}'>Reset Password</a>
                    <p>If you did not request this, you can safely ignore this email.</p>
                    <div class='footer'>
                      &copy; {DateTime.UtcNow.Year} CebuCrust. All rights reserved.
                    </div>
                  </div>
                </body>
                </html>";

            var msg = new MailMessage
            {
                From = new MailAddress(_email, "CebuCrust"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            msg.To.Add(toEmail);

            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(_email, _password),
                EnableSsl = true
            };

            await client.SendMailAsync(msg);
        }
    }
}
