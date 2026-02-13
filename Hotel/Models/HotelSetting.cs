namespace Hotel.Models
{
    public class HotelSetting
    {
        public int Id { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public decimal DeluxePrice { get; set; } = 120;
        public decimal SuitePrice { get; set; } = 250;
        public decimal PresidentialPrice { get; set; } = 500;
        public string? OfferTitle { get; set; }
        public string? OfferDetails { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
