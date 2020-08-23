using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace MainApp.SendGrid
{
    public class SendEmailService
    {
        private readonly string apiKey;

        public SendEmailService(IConfiguration config)
        {
            apiKey = config.GetSection("SendGridKey").Value;
        }

        public async Task SendApplicationAlert(int jobOfferId, string receiverAddress, byte[] fileData)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("290444@pw.edu.pl", "HRMiniApp");
            var subject = "New application";
            var to = new EmailAddress(receiverAddress);
            var plainTextContent = "";
            var htmlContent = $"<strong>Your Job Offer received new Application: <a href=https://localhost:5001/Application/Details/{ jobOfferId }>link</a> </strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var attachment = new Attachment()
            {
                Content = Convert.ToBase64String(fileData),
                Filename = "cv.pdf"
            };
            msg.AddAttachment(attachment);
            await client.SendEmailAsync(msg);
        }
    }
}
