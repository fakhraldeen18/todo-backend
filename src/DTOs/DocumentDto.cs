namespace Harkh_backend.src.DTOs;

public class DocumentReadDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? FromId { get; set; }
    public string Title { get; set; }
    public string FileUrl { get; set; }
    public DateTime UploadedAt { get; set; }
}
public class DocumentCreateDto
{
    public Guid UserId { get; set; }
    public Guid? FromId { get; set; }
    public string Title { get; set; }
    public string FileUrl { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.Now;
}
public class DocumentUpdateDto
{
    public string FileUrl { get; set; }
}
