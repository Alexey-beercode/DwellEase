using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.Domain.Models.Identity;

public class RegisterRequest
{
    [Required] 
    [Display(Name = "Email")] 
    public string Email { get; set; } = null!;

    [Required]
    [FromBody]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = null!;

    [Required]
    [Display(Name = "Имя")]
    public string UserName { get; set; } = null!;

    [Required] 
    [Display(Name = "Роль")] 
    public string Role { get; set; } = null!;
    
    [Required] 
    [Display(Name = "Номер телефона")] 
    public string PhoneNumber { get; set; }
    
}