using System.ComponentModel.DataAnnotations;

namespace Harkh_backend.src.DTOs;

public class EducationReadDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string School { get; set; }
    public string Degree { get; set; }
    public string FieldStudy { get; set; }
    public string Grade { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
public class EducationCreateDto
{
    public Guid UserId { get; set; }
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
public class EducationUpdateDto
{
    public string School { get; set; }
    public string Degree { get; set; }
    public string FieldStudy { get; set; }
    public string Grade { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
