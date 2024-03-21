



using Connect.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Connect.Core.Entities;

public class ReservationProvider
{

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public String PhoneNumber { get; set; }
    public string OwnerId { get; set; }
    [ForeignKey("OwnerId")]
    public Customer Owner { get; set; }
    public List<int>? EmployeesId { get; set; }
    public string? Image { get; set; }
    public string? BackgroundImage { get; set; }
    public string Location { get; set; }
    public List<OfferedService>? OfferedServicesList { get; set; }
    public DateTime DOJ { get; set; }
    public List<string>? FeatureList { get; set; }
    public List<Room>? Rooms { get; set; }
    public DateTime AvailableFrom { get; set; }
    public DateTime AvailableTo { get; set; }
}

