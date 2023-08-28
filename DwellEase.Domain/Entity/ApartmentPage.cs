﻿using DwellEase.Domain.Enum;
using DwellEase.Domain.Models;

namespace DwellEase.Domain.Entity;

public class ApartmentPage
{
    public int Id { get; set; }
    public decimal DaylyPrice { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailableForPurchase { get; set; }
    public ApartmentStatus Status { get; set; }
    public Apartment Apartment { get; set; }
    public PhoneNumber PhoneNumber { get; set; }
    public Guid OwnerId { get; set; }
}