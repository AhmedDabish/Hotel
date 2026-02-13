namespace Hotel.Models
{
    public class Room
    {

        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; } = 2;
        public bool IsAvailable { get; set; } = true;
        public string? Description { get; set; }
        public string? Features { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

    }
}
