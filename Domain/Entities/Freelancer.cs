using Connect.Core.Entities;

namespace Connect.Core.Models
{
    public class Freelancer
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Profession { get; set; }
        public string? Description { get; set; }
        public string OwnerId { get; set; }
        public Customer Owner { get; set; }
        public string? Image { get; set; }
        public string? Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public List<OfferedService>? OfferedServices { get; set; }
        public List<ServiceRequest>? Requests { get; set; }

        public DateTime DOJ { get; set; }
        public virtual List<string>? Skills { get; set; }
        public bool? Availability { get; set; }

    }
}
