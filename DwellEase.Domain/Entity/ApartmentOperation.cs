﻿using System.Net.NetworkInformation;
using DwellEase.Domain.Enum;

namespace DwellEase.Domain.Entity;

public class ApartmentOperation
{
    public Guid Id { get; set; }
    public OperationType OperationType { get; set; }
    public Guid UserId { get; set; }
    public ApartmentPage ApartmentPage { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Price { get; set; }
}