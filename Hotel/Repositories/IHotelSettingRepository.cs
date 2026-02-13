using Hotel.Data;
using Hotel.Models;
using Microsoft.EntityFrameworkCore;
namespace Hotel.Repositories
{
    public interface IHotelSettingRepository
    {
        Task<HotelSetting?> GetAsync();
        Task<HotelSetting> UpdateAsync(HotelSetting settings);
    }
}




namespace Hotel.Repositories
{
    public class HotelSettingRepository : IHotelSettingRepository
    {
        private readonly ApplicationDbContext _context;

        public HotelSettingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HotelSetting?> GetAsync()
        {
            return await _context.HotelSettings.FirstOrDefaultAsync();
        }

        public async Task<HotelSetting> UpdateAsync(HotelSetting settings)
        {
            var existing = await GetAsync();

            if (existing == null)
            {
                await _context.HotelSettings.AddAsync(settings);
            }
            else
            {
                _context.Entry(existing).CurrentValues.SetValues(settings);
                existing.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return settings;
        }
    }
}