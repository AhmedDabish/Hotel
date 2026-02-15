// Services/RoomService.cs
using AutoMapper;
using Hotel.DTOs.Common;
using Hotel.DTOs.Room;
using Hotel.Models;
using Hotel.Repositories;

namespace Hotel.Services
{
    public interface IRoomService
    {
        Task<ApiResponse<List<RoomDto>>> GetAllAsync();
        Task<ApiResponse<RoomDto>> GetByIdAsync(int id);
        Task<ApiResponse<RoomDto>> CreateAsync(CreateRoomDto dto);
        Task<ApiResponse<RoomDto>> UpdateAsync(int id, UpdateRoomDto dto);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse<List<RoomDto>>> GetAvailableAsync();
        Task<ApiResponse<decimal>> GetPriceByTypeAsync(string type);
    }

    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<RoomDto>>> GetAllAsync()
        {
            var rooms = await _roomRepository.GetAllAsync();
            var roomDtos = _mapper.Map<List<RoomDto>>(rooms);
            return ApiResponse<List<RoomDto>>.SuccessResponse(roomDtos);
        }

        public async Task<ApiResponse<RoomDto>> GetByIdAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);

            if (room == null)
            {
                return ApiResponse<RoomDto>.ErrorResponse("الغرفة غير موجودة");
            }

            var roomDto = _mapper.Map<RoomDto>(room);
            return ApiResponse<RoomDto>.SuccessResponse(roomDto);
        }

        public async Task<ApiResponse<RoomDto>> CreateAsync(CreateRoomDto dto)
        {
            var room = _mapper.Map<Room>(dto);
            var createdRoom = await _roomRepository.AddAsync(room);
            var roomDto = _mapper.Map<RoomDto>(createdRoom);

            return ApiResponse<RoomDto>.SuccessResponse(roomDto, "تم إنشاء الغرفة");
        }

        public async Task<ApiResponse<RoomDto>> UpdateAsync(int id, UpdateRoomDto dto)
        {
            var room = await _roomRepository.GetByIdAsync(id);

            if (room == null)
            {
                return ApiResponse<RoomDto>.ErrorResponse("الغرفة غير موجودة");
            }

            if (!string.IsNullOrEmpty(dto.Name))
            {
                room.Name = dto.Name;
            }

            if (dto.PricePerNight.HasValue)
            {
                room.PricePerNight = dto.PricePerNight.Value;
            }

            if (dto.Capacity.HasValue)
            {
                room.Capacity = dto.Capacity.Value;
            }

            if (dto.IsAvailable.HasValue)
            {
                room.IsAvailable = dto.IsAvailable.Value;
            }

            if (!string.IsNullOrEmpty(dto.Description))
            {
                room.Description = dto.Description;
            }

            if (!string.IsNullOrEmpty(dto.Features))
            {
                room.Features = dto.Features;
            }

            await _roomRepository.UpdateAsync(room);

            var updatedDto = _mapper.Map<RoomDto>(room);
            return ApiResponse<RoomDto>.SuccessResponse(updatedDto, "تم تحديث الغرفة");
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);

            if (room == null)
            {
                return ApiResponse.ErrorResponse("الغرفة غير موجودة");
            }

            await _roomRepository.DeleteAsync(room);
            return ApiResponse.SuccessResponse("تم حذف الغرفة");
        }

        public async Task<ApiResponse<List<RoomDto>>> GetAvailableAsync()
        {
            var rooms = await _roomRepository.GetAvailableAsync();
            var roomDtos = _mapper.Map<List<RoomDto>>(rooms);
            return ApiResponse<List<RoomDto>>.SuccessResponse(roomDtos);
        }

        public async Task<ApiResponse<decimal>> GetPriceByTypeAsync(string type)
        {
            var price = await _roomRepository.GetPriceByTypeAsync(type);
            return ApiResponse<decimal>.SuccessResponse(price);
        }
    }
}