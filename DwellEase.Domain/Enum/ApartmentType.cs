using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace DwellEase.Domain.Enum;

[SwaggerSchema(description:"Enum : 0 - Cottege, 1 - Room, 2 - Flat")]
public enum ApartmentType
{
    [Display(Name = "Коттедж")]
    Cottege,
    [Display(Name = "Комната")]
    Room,
    [Display(Name = "Квартира")]
    Flat
}