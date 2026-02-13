using Hotel.Data;
using Hotel.Models;



using Microsoft.EntityFrameworkCore;
namespace Hotel.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int id);
        Task<List<Booking>> GetAllAsync();
        Task<List<Booking>> GetByStatusAsync(string status);
        Task<List<Booking>> GetRecentAsync(int count);
        Task<Booking> AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(Booking booking);
        Task<decimal> GetTotalRevenueAsync();
        Task<int> GetCountAsync();
        Task<bool> IsRoomAvailableAsync(string roomType, DateTime checkIn, DateTime checkOut);
    }
}


namespace Hotel.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetByStatusAsync(string status)
        {
            return await _context.Bookings
                .Where(b => b.Status == status)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetRecentAsync(int count)
        {
            return await _context.Bookings
                .OrderByDescending(b => b.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Booking> AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Booking booking)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Bookings
                .Where(b => b.Status == "Confirmed" || b.Status == "Paid")
                .SumAsync(b => b.TotalAmount);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Bookings.CountAsync();
        }

        public async Task<bool> IsRoomAvailableAsync(string roomType, DateTime checkIn, DateTime checkOut)
        {
            return !await _context.Bookings.AnyAsync(b =>
                b.RoomType == roomType &&
                b.Status != "Cancelled" &&
                b.CheckInDate < checkOut &&
                b.CheckOutDate > checkIn);
        }
    }
}