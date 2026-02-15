// Controllers/HotelController.cs
using Hotel.DTOs.Hotel;
using Hotel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet("settings")]
        public async Task<IActionResult> GetSettings()
        {
            var result = await _hotelService.GetSettingsAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("settings")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSettings([FromBody] UpdateHotelSettingDto dto)
        {
            var result = await _hotelService.UpdateSettingsAsync(dto);
            return StatusCode(result.StatusCode, result);
        }
    }
}