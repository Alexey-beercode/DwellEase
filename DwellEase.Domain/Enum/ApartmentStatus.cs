using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace DwellEase.Domain.Enum;

[SwaggerSchema(description:"Enum : 0 - Available, 1 - Bought, 2 - Rented")]
public enum ApartmentStatus
{
    [Display(Name = "Доступно")]
    Available,
    [Display(Name = "Куплено")]
    Bought,
    [Display(Name = "Арендовано")]
    Rented
}