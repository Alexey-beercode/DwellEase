using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace DwellEase.Domain.Enum;

[SwaggerSchema(description:"Enum : 0 - Pending, 1 - Approved, 2 - Rejected")]
public enum ListingApprovalStatus
{
    [Display(Name = "В обработке")]
    Pending,
    [Display(Name = "Проверено")]
    Approved,
    [Display(Name = "Отклонено")]
    Rejected 
}