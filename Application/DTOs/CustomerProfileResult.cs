namespace Connect.Application.DTOs
{
    public class CustomerProfileResult
    {
        public string Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }

        public string? Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }

    }
}
