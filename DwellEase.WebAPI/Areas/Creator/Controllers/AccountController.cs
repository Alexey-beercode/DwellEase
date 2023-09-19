using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.WebAPI.Areas.Creator.Controllers;

[Area("Creator")]
[Authorize(Policy = "CreatorArea")]
public class AccountController:ControllerBase
{
    
}