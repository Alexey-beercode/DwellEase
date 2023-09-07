using System.ComponentModel.DataAnnotations;

namespace DwellEase.Domain.Enum;

public enum OperationType
{
    [Display(Name = "Покупка")]
    Purchase,
    [Display(Name = "Аренда")]
    Rent
}