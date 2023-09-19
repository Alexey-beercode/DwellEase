using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.WebAPI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "AdminArea")]
public class AccountController:ControllerBase
{
    
}