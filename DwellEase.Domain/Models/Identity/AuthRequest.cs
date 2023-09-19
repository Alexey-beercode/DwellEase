using System.ComponentModel.DataAnnotations;

namespace DwellEase.Domain.Models.Identity;

public class AuthRequest
{
    [EmailAddress]
    public string Email { get; set; } = null!;
    [MinLength(5)]
    public string Password { get; set; } = null!;
}