// Services/HotelService.cs
using AutoMapper;
using Hotel.DTOs.Common;
using Hotel.DTOs.Hotel;
using Hotel.Models;
using Hotel.Repositories;

namespace Hotel.Services
{
    public interface IHotelService
    {
        Task<ApiResponse<HotelSettingDto>> GetSettingsAsync();
        Task<ApiResponse<HotelSettingDto>> UpdateSettingsAsync(UpdateHotelSettingDto dto);
    }

    public class HotelService : IHotelService
    {
        private readonly IHotelSettingRepository _hotelSettingRepository;
        private readonly IMapper _mapper;

        public HotelService(IHotelSettingRepository hotelSettingRepository, IMapper mapper)
        {
            _hotelSettingRepository = hotelSettingRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<HotelSettingDto>> GetSettingsAsync()
        {
            var settings = await _hotelSettingRepository.GetAsync();

            if (settings == null)
            {
                settings = new HotelSetting
                {
                    HotelName = "فندق الرفاهية",
                    DeluxePrice = 120,
                    SuitePrice = 250,
                    PresidentialPrice = 500
                };

                settings = await _hotelSettingRepository.UpdateAsync(settings);
            }

            var settingsDto = _mapper.Map<HotelSettingDto>(settings);
            return ApiResponse<HotelSettingDto>.SuccessResponse(settingsDto);
        }

        public async Task<ApiResponse<HotelSettingDto>> UpdateSettingsAsync(UpdateHotelSettingDto dto)
        {
            var existing = await _hotelSettingRepository.GetAsync();
            var settings = existing ?? new HotelSetting();

            if (!string.IsNullOrEmpty(dto.HotelName))
            {
                settings.HotelName = dto.HotelName;
            }

            if (!string.IsNullOrEmpty(dto.Address))
            {
                settings.Address = dto.Address;
            }

            if (!string.IsNullOrEmpty(dto.Phone))
            {
                settings.Phone = dto.Phone;
            }

            if (!string.IsNullOrEmpty(dto.Email))
            {
                settings.Email = dto.Email;
            }

            if (!string.IsNullOrEmpty(dto.Description))
            {
                settings.Description = dto.Description;
            }

            if (dto.DeluxePrice.HasValue)
            {
                settings.DeluxePrice = dto.DeluxePrice.Value;
            }

            if (dto.SuitePrice.HasValue)
            {
                settings.SuitePrice = dto.SuitePrice.Value;
            }

            if (dto.PresidentialPrice.HasValue)
            {
                settings.PresidentialPrice = dto.PresidentialPrice.Value;
            }

            if (!string.IsNullOrEmpty(dto.OfferTitle))
            {
                settings.OfferTitle = dto.OfferTitle;
            }

            if (!string.IsNullOrEmpty(dto.OfferDetails))
            {
                settings.OfferDetails = dto.OfferDetails;
            }

            settings.UpdatedAt = DateTime.UtcNow;

            var updatedSettings = await _hotelSettingRepository.UpdateAsync(settings);
            var settingsDto = _mapper.Map<HotelSettingDto>(updatedSettings);

            return ApiResponse<HotelSettingDto>.SuccessResponse(settingsDto, "تم تحديث إعدادات الفندق");
        }
    }
}