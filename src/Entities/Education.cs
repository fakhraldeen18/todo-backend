using System.ComponentModel.DataAnnotations;

namespace Harkh_backend.src.Entities;

public class Education
{


    public Guid Id { get; set; }
    public Guid UserId { get; set; } // foreign key
    [Required]
    public string School { get; set; }
    [Required]
    public string Degree { get; set; }
    [Required]
    public string FieldStudy { get; set; }
    public string? Grade { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }

}
