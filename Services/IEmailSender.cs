using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, string cc = "");
        void SendEmail(string email, string subject, string message, string cc = "");
    }
}
