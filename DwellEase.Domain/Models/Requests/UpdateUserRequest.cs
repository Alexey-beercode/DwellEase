namespace DwellEase.Domain.Models.Requests;

public class UpdateUserRequest
{
  public string UserId { get; set; }
  public string Email { get; set; }
  public string UserName { get; set; }
  public string Password { get; set; }
  public string NewPassword { get; set; }
  public string PhoneNumber { get; set; }
}