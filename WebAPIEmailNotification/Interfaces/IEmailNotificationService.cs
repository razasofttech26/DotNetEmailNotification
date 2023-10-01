using WebAPIEmailNotification.Models;

namespace WebAPIEmailNotification.Interfaces
{
    public interface IEmailNotificationService
    {
        Task SendEmailAsync(EmailModel emailModel);
    }
}
