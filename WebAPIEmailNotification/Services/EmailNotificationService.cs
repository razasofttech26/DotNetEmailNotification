using System;
using System.Net.Mail;
using WebAPIEmailNotification.Interfaces;
using WebAPIEmailNotification.Models;
using System.Net;
using System.IO;

namespace WebAPIEmailNotification.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly string recipientEmail = "razasofttech26@gmail.com";
        private readonly string subject = "Test Email";
        private readonly string InternalMailBody = "<!DOCTYPE html><html lang='en'><head><meta charset='UTF-8'><meta name='viewport' content='width=device-width,initial-scale=1'><title>Contact Us Lead</title><style>body,h1,p{margin:0;padding:0}.container{width:100%;max-width:600px;margin:0 auto;background-color:#685d5d;padding:20px}.header{text-align:center;margin-bottom:20px}.logo img{max-width:150px}.content{background-color:#fff;padding:20px;border-radius:5px}.lead-details{margin-top:20px}.footer{text-align:center;margin-top:20px}</style></head><body><div class='container'><div class='header'><div class='logo'><img src='https://razasofttech.com/assets/img/white.png' alt='Company Logo'></div><h1>Contact Us Lead</h1></div><div class='content'><p><strong>Subject:</strong> {{subject}}</p><div class='lead-details'><p><strong>Client Name:</strong> {{name}}</p><p><strong>Mobile Number:</strong> {{number}}</p><p><strong>Client Email:</strong> {{email}}</p><p><strong>Client Description:</strong> {{description}}</p></div></div><div class='footer'><p>&copy; 2023 Raza Software and Technologies.</p></div></div></body></html>";
        private readonly string ThankyouMailBody = "<!DOCTYPE html><html lang='en'><head><meta charset='UTF-8'><meta name='viewport' content='width=device-width,initial-scale=1'><title>Thank You for Contacting Us</title><style>body,h1,p{margin:0;padding:0}.container{width:100%;max-width:600px;margin:0 auto;background-color:#685d5d;padding:20px}.header{text-align:center;margin-bottom:20px}.logo img{max-width:150px}.content{background-color:#fff;padding:20px;border-radius:5px}.message{margin-top:20px}.footer{text-align:center;margin-top:20px}</style></head><body><div class='container'><div class='header'><div class='logo'><img src='https://razasofttech.com/assets/img/white.png' alt='Company Logo'></div></div><div class='content'><p>Dear {{name}},</p><p>Thank you for reaching out to us through our website. We appreciate your interest and will respond to your inquiry shortly.</p><div class='message'><p>Best regards,</p><p>Raza Software Technologies.</p><p>Contact us: +919619756588</p><p><a href='https://razasofttech.com/' target='_blank'>Raza Software & Technologies</a></p></div></div><div class='footer'><p>&copy; 2023 Raza Software & Technologies.</p></div></div></body></html>";
        private readonly IConfiguration _configuration;
        private string smtpServer;
        private string smtpUsername;
        private string smtpPassword;
        private int smtpPort;
        public EmailNotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
            smtpServer = _configuration["SmtpSettings:Server"];
            smtpPort = Convert.ToInt32(_configuration["SmtpSettings:Port"]);
            smtpUsername = _configuration["SmtpSettings:smtpUsername"];
            smtpPassword = _configuration["SmtpSettings:smtpPassword"];
        }

        public async Task SendEmailAsync(EmailModel emailModel)
        {
            try
            {
                await sendThankyouMailAsync(emailModel);

                await SendEmailAsyncInternal(emailModel);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task sendThankyouMailAsync(EmailModel emailModel)
        {

            try
            {
                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    string emailbody = ThankyouMailBody.Replace("{{name}}", emailModel.CustomerName);
                    using (var mail = new MailMessage(smtpUsername, emailModel.CustomerEmail, "Thank You for Contacting Us", emailbody))
                    {
                        mail.IsBodyHtml = true;
                        await client.SendMailAsync(mail);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendEmailAsyncInternal(EmailModel emailModel)
        {
            try
            {
                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                   
                    string emailbody = InternalMailBody.Replace("{{subject}}", emailModel.Subject).Replace("{{name}}", emailModel.CustomerName).Replace("{{number}}", Convert.ToString(emailModel.CustomerPhone)).Replace("{{email}}", emailModel.CustomerEmail).Replace("{{description}}", emailModel.Description);
                    using (var mail = new MailMessage(emailModel.CustomerEmail, smtpUsername, emailModel.Subject, emailbody))
                    {
                        mail.IsBodyHtml = true;
                        mail.CC.Add("mehfooz528@gmail.com");
                        await client.SendMailAsync(mail);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}
