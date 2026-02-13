// Helpers/MappingProfile.cs
using AutoMapper;
using Hotel.DTOs.Auth;
using Hotel.DTOs.Booking;
using Hotel.DTOs.Hotel;
using Hotel.DTOs.Room;
using Hotel.Models;

namespace Hotel.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Auth mappings
            CreateMap<User, UserDto>();

            // Booking mappings
            CreateMap<Booking, BookingDto>();
            CreateMap<CreateBookingDto, Booking>();

            // Room mappings
            CreateMap<Room, RoomDto>();
            CreateMap<CreateRoomDto, Room>();

            // Hotel settings mappings
            CreateMap<HotelSetting, HotelSettingDto>();
            CreateMap<UpdateHotelSettingDto, HotelSetting>();
        }
    }
}