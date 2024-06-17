using Connect.Application.Settings;
using Microsoft.AspNetCore.Http;

namespace Connect.Application.Settings
{
    public interface IMailingService
    {   
        void SendMail(MailMessage message);

    }
}
