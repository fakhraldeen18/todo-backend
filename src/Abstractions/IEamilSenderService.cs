using Harkh_backend.src.Entities;

namespace Harkh_backend.src.Abstractions;

public interface IEmailSenderService

{
    public  Task<bool> SendEmailAsync(EmailSender emailRequest);
}
