using Hotel.Data;
using Hotel.Models;

using Microsoft.EntityFrameworkCore;
namespace Hotel.Repositories
{
    public interface IRoomRepository
    {
        Task<Room?> GetByIdAsync(int id);
        Task<List<Room>> GetAllAsync();
        Task<List<Room>> GetAvailableAsync();
        Task<List<Room>> GetByTypeAsync(string type);
        Task<Room> AddAsync(Room room);
        Task UpdateAsync(Room room);
        Task DeleteAsync(Room room);
        Task<decimal> GetPriceByTypeAsync(string type);
    }
}



namespace Hotel.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task<List<Room>> GetAllAsync()
        {
            return await _context.Rooms.ToListAsync();
        }

        public async Task<List<Room>> GetAvailableAsync()
        {
            return await _context.Rooms
                .Where(r => r.IsAvailable)
                .ToListAsync();
        }

        public async Task<List<Room>> GetByTypeAsync(string type)
        {
            return await _context.Rooms
                .Where(r => r.Type == type && r.IsAvailable)
                .ToListAsync();
        }

        public async Task<Room> AddAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Room room)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetPriceByTypeAsync(string type)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Type == type);
            return room?.PricePerNight ?? 0;
        }
    }
}