// DTOs/Room/RoomDto.cs
namespace Hotel.DTOs.Room
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }
        public string? Description { get; set; }
        public string? Features { get; set; }
    }

    public class CreateRoomDto
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; } = 2;
        public string? Description { get; set; }
        public string? Features { get; set; }
    }

    public class UpdateRoomDto
    {
        public string? Name { get; set; }
        public decimal? PricePerNight { get; set; }
        public int? Capacity { get; set; }
        public bool? IsAvailable { get; set; }
        public string? Description { get; set; }
        public string? Features { get; set; }
    }
}