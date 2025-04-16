using Harkh_backend.src.Abstractions;
using Harkh_backend.src.Entities;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Harkh_backend.src.Services;

public class EmailSenderService : IEmailSenderService
{

    private readonly IConfiguration _configuration;

    public EmailSenderService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> SendEmailAsync(EmailSender emailRequest)
    {
        var apiKey = _configuration["SendGrid:ApiKey"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(
            _configuration["SendGrid:FromEmail"],
            _configuration["SendGrid:FromName"]);

        var to = new EmailAddress(emailRequest.ToEmail);
        var msg = MailHelper.CreateSingleEmail(
            from,
            to,
            emailRequest.Subject,
            emailRequest.PlainTextContent,
            emailRequest.HtmlContent);

        var response = await client.SendEmailAsync(msg);
        return response.IsSuccessStatusCode;
    }
}
