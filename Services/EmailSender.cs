using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;

namespace ASJ.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            this.logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string message, string cc = "")
        {
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.PickupDirectoryLocation = "C:\\RTI\\ASJ\\Emails\\";
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("doj-dcra@rti.org");
            mailMessage.To.Add(email);
            if(cc != "")
                mailMessage.CC.Add(cc);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            client.Send(mailMessage);
            return Task.CompletedTask;
        }

        public void SendEmail(string email, string subject, string message, string cc = "")
        {
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = "rtismtp.rti.org";
            //client.PickupDirectoryLocation = "C:\\RTI\\ASJ\\Emails\\";
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("doj-dcra@rti.org");
            mailMessage.To.Add(email);
            if (cc != "")
                mailMessage.CC.Add(cc);

            mailMessage.Body = message;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = subject;
            client.Send(mailMessage);
            
        }
    }
}
