﻿using Connect.Core.Models;
namespace Connect.Core.Entities;

public class ReservationProvider
{

    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string Description { get; set; }
    public String PhoneNumber { get; set; }
    public string OwnerId { get; set; }
    public virtual Customer Owner { get; set; }
    public List<int>? EmployeesId { get; set; }
    public string? Image { get; set; }
    public string? BackgroundImage { get; set; }
    public string? Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    //public virtual List<OfferedService>? OfferedServicesList { get; set; }
    public DateTime DOJ { get; set; }
    public List<string>? FeatureList { get; set; }
   // public virtual List<Room>? Rooms { get; set; }
    public bool? Availability { get; set; }
}

