// DTOs/Hotel/HotelSettingDto.cs
namespace Hotel.DTOs.Hotel
{
    public class HotelSettingDto
    {
        public int Id { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public decimal DeluxePrice { get; set; }
        public decimal SuitePrice { get; set; }
        public decimal PresidentialPrice { get; set; }
        public string? OfferTitle { get; set; }
        public string? OfferDetails { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateHotelSettingDto
    {
        public string? HotelName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public decimal? DeluxePrice { get; set; }
        public decimal? SuitePrice { get; set; }
        public decimal? PresidentialPrice { get; set; }
        public string? OfferTitle { get; set; }
        public string? OfferDetails { get; set; }
    }
}