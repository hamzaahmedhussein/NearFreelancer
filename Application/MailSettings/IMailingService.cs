using Connect.Application.MailSettings;
using Microsoft.AspNetCore.Http;

namespace Connect.Application.Settings
{
    public interface IMailingService
    {   
        void SendMail(Message message);

    }
}
