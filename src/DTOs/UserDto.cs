using System.ComponentModel.DataAnnotations;
using Harkh_backend.src.Enums;

namespace Harkh_backend.src.DTOs;

public class UserReadDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public string? Position { get; set; }
    public string? Nationality { get; set; }
    public string ProfileImage { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UserCreateDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
public class UserInviteCreateDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
public class UserLogInDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}

public class UserUpdateProfileDto
{
    public string? Name { get; set; }
    public string? Position { get; set; }
    public string? ProfileImage { get; set; }
}


public class UserUpdatePersonalInfoDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Nationality { get; set; }
    [Required]
    public DateTime BirthDate { get; set; }
    [Required]
    public string Phone { get; set; }
}

public class UserUpdateRoleDto
{
    [Required]
    public Role Role { get; set; }
}
public class UserInviteUpdatePassWordDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Password { get; set; }
}
public class UserInviteEmailDto
{
    [Required]
    public string Email { get; set; }
}