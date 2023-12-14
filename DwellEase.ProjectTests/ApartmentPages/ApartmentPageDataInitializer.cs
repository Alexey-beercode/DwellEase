using DwellEase.Domain.Entity;
using DwellEase.Domain.Enum;
using DwellEase.Domain.Models;
using System;
using System.Collections.Generic;
using DwellEase.ProjectTests.Common.Interfaces;
using MongoDB.Driver;
using Moq;

namespace DwellEase.ProjectTests.ApartmentPages
{
    public class ApartmentPageDataInitializer:IDataInitializer<ApartmentPage>
    {
        public void Initial(ref IMongoCollection<ApartmentPage> collection)
        {
            var pages = new List<ApartmentPage>()
            {
                new ApartmentPage()
                {
                    Id = Guid.NewGuid(),
                    OwnerId = Guid.NewGuid(),
                    Apartment = new Apartment()
                    {
                        Area = 80,
                        ApartmentType = ApartmentType.Room,
                        Rooms = 4,
                        Address = new Address()
                        {
                            Street = "Yakubova",
                            HouseNumber = 481,
                            Building = "4",
                            City = "Minsk"
                        }
                    },
                    Title = "Квартира в Минске",
                    PriorityType = PriorityType.Standart,
                    Date = DateOnly.FromDateTime(DateTime.Today),
                    Status = ApartmentStatus.Available,
                    ApprovalStatus = ListingApprovalStatus.Approved,
                    IsAvailableForPurchase = true,
                    Price = 90000,
                    DaylyPrice = 600,
                    PhoneNumber = new PhoneNumber("+375445983720"),
                    Images = new List<Image>() { new Image() { ContentType = "IMG" } }
                },
                new ApartmentPage()
                {
                    Id = Guid.NewGuid(),
                    OwnerId = Guid.NewGuid(),
                    Apartment = new Apartment()
                    {
                        Area = 100,
                        ApartmentType = ApartmentType.Room,
                        Rooms = 1,
                        Address = new Address()
                        {
                            Street = "Lenina",
                            HouseNumber = 5,
                            Building = "A",
                            City = "Minsk"
                        }
                    },
                    Title = "Студия в Минске",
                    PriorityType = PriorityType.High,
                    Date = DateOnly.FromDateTime(DateTime.Today),
                    Status = ApartmentStatus.Available,
                    ApprovalStatus = ListingApprovalStatus.Approved,
                    DaylyPrice = 500,
                    IsAvailableForPurchase = true,
                    Price = 100000,
                    PhoneNumber = new PhoneNumber("+375445983750"),
                    Images = new List<Image>() { new Image() { ContentType = "IMG" } }
                },
                new ApartmentPage()
                {
                    Id = Guid.NewGuid(),
                    OwnerId = Guid.NewGuid(),
                    Apartment = new Apartment()
                    {
                        Area = 120,
                        ApartmentType = ApartmentType.Cottege,
                        Rooms = 2,
                        Address = new Address()
                        {
                            Street = "Kalinina",
                            HouseNumber = 10,
                            Building = "B",
                            City = "Minsk"
                        }
                    },
                    Title = "Двухкомнатная квартира в Минске",
                    PriorityType = PriorityType.Standart,
                    Date = DateOnly.FromDateTime(DateTime.Today),
                    Status = ApartmentStatus.Available,
                    ApprovalStatus = ListingApprovalStatus.Pending,
                    DaylyPrice = 800,
                    IsAvailableForPurchase = true,
                    Price = 120000,
                    PhoneNumber = new PhoneNumber("+375445983760"),
                    Images = new List<Image>() { new Image() { ContentType = "IMG" } }
                },
                new ApartmentPage()
                {
                    Id = Guid.NewGuid(),
                    OwnerId = Guid.NewGuid(),
                    Apartment = new Apartment()
                    {
                        Area = 140,
                        ApartmentType = ApartmentType.Flat,
                        Rooms = 3,
                        Address = new Address()
                        {
                            Street = "Nezavisimosti",
                            HouseNumber = 55,
                            Building = "C",
                            City = "Minsk"
                        }
                    },
                    Title = "Трехкомнатная квартира в Минске",
                    PriorityType = PriorityType.High,
                    Date = DateOnly.FromDateTime(DateTime.Today),
                    Status = ApartmentStatus.Available,
                    ApprovalStatus = ListingApprovalStatus.Approved,
                    DaylyPrice = 1100,
                    IsAvailableForPurchase = true,
                    Price = 150000,
                    PhoneNumber = new PhoneNumber("+375445983770")
                }
            };
            collection.InsertMany(pages);
        }
    }
}