using System.ComponentModel.DataAnnotations;

namespace DwellEase.Domain.Enum;

public enum PriorityType
{
    [Display(Name = "Стандартный приоритет")]
    Standart,
    [Display(Name = "Повышенный приоритет")]
    Elevate,
    [Display(Name = "Высокий приоритет")]
    High
}