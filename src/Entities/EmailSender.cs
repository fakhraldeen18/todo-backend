
namespace Harkh_backend.src.Entities;

public class EmailSender
{
    public Guid SenderID { get; set; } // Foreign key
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string PlainTextContent { get; set; }
    public string HtmlContent { get; set; }
}
