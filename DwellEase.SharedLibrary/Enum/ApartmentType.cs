using System.ComponentModel.DataAnnotations;

namespace DwellEase.Domain.Enum;

public enum ApartmentType
{
    [Display(Name = "Коттедж")]
    Cottege,
    [Display(Name = "Комната")]
    Room,
    [Display(Name = "Квартира")]
    Flat
}