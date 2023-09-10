using System.ComponentModel.DataAnnotations;

namespace DwellEase.Domain.Enum;

public enum ListingApprovalStatus
{
    [Display(Name = "В обработке")]
    Pending,
    [Display(Name = "Проверено")]
    Approved,
    [Display(Name = "Отклонено")]
    Rejected 
}