using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.Domain.Models.Identity;

public class AuthRequest
{
    public string UserName { get; set; } = null!;
    [FromBody]
    public string Password { get; set; } = null!;
}