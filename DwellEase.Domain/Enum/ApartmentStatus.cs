using System.ComponentModel.DataAnnotations;

namespace DwellEase.Domain.Enum;

public enum ApartmentStatus
{
    [Display(Name = "Доступно")]
    Available,
    [Display(Name = "Куплено")]
    Bought,
    [Display(Name = "Арендовано")]
    Rented
}