using Microsoft.AspNetCore.Http;

namespace Connect.Application.Settings
{
    public interface IMailingService
    {
        Task SendMailAsync(string mailTo, string subject, string body, IList<IFormFile> attachment = null);

    }
}
