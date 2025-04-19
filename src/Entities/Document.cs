
namespace Harkh_backend.src.Entities;

public class Document
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? FromId { get; set; }

    public string Title { get; set; }
    public string FileUrl { get; set; }
    public DateTime UploadedAt { get; set; }
}
