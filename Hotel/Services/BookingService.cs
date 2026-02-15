// Services/BookingService.cs
using AutoMapper;
using Hotel.DTOs.Booking;
using Hotel.DTOs.Common;
using Hotel.Models;
using Hotel.Repositories;

namespace Hotel.Services
{
    public interface IBookingService
    {
        Task<ApiResponse<List<BookingDto>>> GetAllAsync();
        Task<ApiResponse<BookingDto>> GetByIdAsync(int id);
        Task<ApiResponse<BookingDto>> CreateAsync(CreateBookingDto dto);
        Task<ApiResponse<BookingDto>> UpdateAsync(int id, UpdateBookingDto dto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse<BookingStatsDto>> GetStatsAsync();
        Task<ApiResponse<List<BookingDto>>> GetRecentAsync(int count);
    }

    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public BookingService(
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<BookingDto>>> GetAllAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            var bookingDtos = _mapper.Map<List<BookingDto>>(bookings);
            return ApiResponse<List<BookingDto>>.SuccessResponse(bookingDtos);
        }

        public async Task<ApiResponse<BookingDto>> GetByIdAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);

            if (booking == null)
            {
                return ApiResponse<BookingDto>.ErrorResponse("الحجز غير موجود");
            }

            var bookingDto = _mapper.Map<BookingDto>(booking);
            return ApiResponse<BookingDto>.SuccessResponse(bookingDto);
        }

        public async Task<ApiResponse<BookingDto>> CreateAsync(CreateBookingDto dto)
        {
            var rooms = await _roomRepository.GetByTypeAsync(dto.RoomType);

            if (!rooms.Any())
            {
                return ApiResponse<BookingDto>.ErrorResponse("نوع الغرفة غير متاح");
            }

            var isAvailable = await _bookingRepository.IsRoomAvailableAsync(
                dto.RoomType, dto.CheckInDate, dto.CheckOutDate);

            if (!isAvailable)
            {
                return ApiResponse<BookingDto>.ErrorResponse("الغرفة محجوزة في هذه الفترة");
            }

            var roomPrice = await _roomRepository.GetPriceByTypeAsync(dto.RoomType);
            var nights = (dto.CheckOutDate - dto.CheckInDate).Days;

            var booking = _mapper.Map<Booking>(dto);
            booking.TotalAmount = roomPrice * nights;

            var createdBooking = await _bookingRepository.AddAsync(booking);
            var bookingDto = _mapper.Map<BookingDto>(createdBooking);

            return ApiResponse<BookingDto>.SuccessResponse(bookingDto, "تم إنشاء الحجز بنجاح");
        }

        public async Task<ApiResponse<BookingDto>> UpdateAsync(int id, UpdateBookingDto dto)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);

            if (booking == null)
            {
                return ApiResponse<BookingDto>.ErrorResponse("الحجز غير موجود");
            }

            if (!string.IsNullOrEmpty(dto.Status))
            {
                booking.Status = dto.Status;
            }

            if (dto.IsPaid.HasValue)
            {
                booking.IsPaid = dto.IsPaid.Value;
            }

            if (!string.IsNullOrEmpty(dto.SpecialRequests))
            {
                booking.SpecialRequests = dto.SpecialRequests;
            }

            await _bookingRepository.UpdateAsync(booking);

            var updatedDto = _mapper.Map<BookingDto>(booking);
            return ApiResponse<BookingDto>.SuccessResponse(updatedDto, "تم تحديث الحجز");
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);

            if (booking == null)
            {
                return ApiResponse.ErrorResponse("الحجز غير موجود");
            }

            await _bookingRepository.DeleteAsync(booking);
            return ApiResponse.SuccessResponse("تم حذف الحجز");
        }

        public async Task<ApiResponse<BookingStatsDto>> GetStatsAsync()
        {
            var totalBookings = await _bookingRepository.GetCountAsync();
            var confirmedBookings = await _bookingRepository.GetByStatusAsync("Confirmed");
            var pendingBookings = await _bookingRepository.GetByStatusAsync("Pending");
            var totalRevenue = await _bookingRepository.GetTotalRevenueAsync();

            var monthlyRevenue = (await _bookingRepository.GetAllAsync())
                .Where(b => b.CreatedAt.Month == DateTime.UtcNow.Month)
                .Sum(b => b.TotalAmount);

            return ApiResponse<BookingStatsDto>.SuccessResponse(new BookingStatsDto
            {
                TotalBookings = totalBookings,
                ConfirmedBookings = confirmedBookings.Count,
                PendingBookings = pendingBookings.Count,
                TotalRevenue = totalRevenue,
                MonthlyRevenue = monthlyRevenue
            });
        }

        public async Task<ApiResponse<List<BookingDto>>> GetRecentAsync(int count)
        {
            var bookings = await _bookingRepository.GetRecentAsync(count);
            var bookingDtos = _mapper.Map<List<BookingDto>>(bookings);
            return ApiResponse<List<BookingDto>>.SuccessResponse(bookingDtos);
        }
    }
}