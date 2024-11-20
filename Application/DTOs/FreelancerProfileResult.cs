namespace Connect.Application.DTOs
{
    public class FreelancerProfileResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Profession { get; set; }
        public string Image { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string? Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public List<string> Skills { get; set; }
        public bool Availability { get; set; }
    }
}
