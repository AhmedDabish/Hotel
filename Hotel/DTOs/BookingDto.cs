// DTOs/Booking/BookingDto.cs
namespace Hotel.DTOs.Booking
{
    public class BookingDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Nights => (CheckOutDate - CheckInDate).Days;
    }

    public class CreateBookingDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; } = 1;
        public string? SpecialRequests { get; set; }
    }

    public class UpdateBookingDto
    {
        public string? Status { get; set; }
        public bool? IsPaid { get; set; }
        public string? SpecialRequests { get; set; }
    }

    public class BookingStatsDto
    {
        public int TotalBookings { get; set; }
        public int ConfirmedBookings { get; set; }
        public int PendingBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
    }
}