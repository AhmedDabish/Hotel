# 🏨 Hotel Management System — REST API

> A production-ready hotel management backend built with ASP.NET Core 9, featuring JWT authentication, room management, booking engine, and a full admin dashboard API.

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-9.0-512BD4?style=flat&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-13-239120?style=flat&logo=csharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=flat&logo=microsoftsqlserver&logoColor=white)
![Entity Framework](https://img.shields.io/badge/EF_Core-9.0-512BD4?style=flat&logo=dotnet&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-Auth-000000?style=flat&logo=jsonwebtokens&logoColor=white)
![AutoMapper](https://img.shields.io/badge/AutoMapper-Enabled-BE4B48?style=flat)
![BCrypt](https://img.shields.io/badge/BCrypt-Password_Hashing-4A90D9?style=flat)

---

## 📖 About the project

A fully featured **Hotel Management REST API** built with clean architecture principles. It handles room reservations, room management, hotel settings, admin authentication, and file uploads — all exposed through a secure, role-based API.

Designed to power any frontend (React, Angular, Vue, or mobile) with a consistent `ApiResponse<T>` wrapper on every endpoint.

---

## ✨ Features

- 🔐 **JWT Authentication** with role-based access control (`Admin` role)
- 📅 **Booking Engine** — create, update, cancel bookings with availability checking
- 🛏️ **Room Management** — CRUD for rooms with type, capacity, pricing, and image support
- 📊 **Booking Statistics** — total bookings, confirmed, pending, revenue (total & monthly)
- ⚙️ **Hotel Settings** — manage hotel name, contact info, room pricing, and offers
- 🖼️ **Image Upload** — upload room images with file validation (jpg, png, webp, avif...)
- 🧱 **Clean Architecture** — Controllers → Services → Repositories → DbContext
- 🔄 **AutoMapper** — clean separation between Models and DTOs
- 🔒 **BCrypt** — secure password hashing

---

## 🧰 Tech stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 9 |
| Language | C# 13 |
| Database | SQL Server |
| ORM | Entity Framework Core 9 |
| Auth | JWT Bearer Tokens |
| Password Hashing | BCrypt.Net |
| Object Mapping | AutoMapper |
| Architecture | Repository Pattern + Service Layer |

---

## 🗂️ Project structure

```
Hotel/
├── Controllers/
│   ├── AuthController.cs        — login & token generation
│   ├── BookingsController.cs    — booking CRUD + stats
│   ├── RoomsController.cs       — room CRUD + availability
│   ├── HotelController.cs       — hotel settings (admin only)
│   └── UploadController.cs      — room image uploads
│
├── Services/
│   ├── AuthService.cs           — JWT generation, login logic
│   ├── BookingService.cs        — booking business logic
│   ├── RoomService.cs           — room business logic
│   └── HotelService.cs          — settings management
│
├── Repositories/
│   ├── BookingRepository.cs     — booking data access
│   ├── RoomRepository.cs        — room data access
│   ├── HotelSettingRepository.cs
│   └── UserRepository.cs        — user data access + password verify
│
├── Models/
│   ├── User.cs
│   ├── Booking.cs
│   ├── Room.cs
│   └── HotelSetting.cs
│
├── DTOs/
│   ├── Auth/                    — LoginDto, LoginResponseDto, UserDto
│   ├── Booking/                 — BookingDto, CreateBookingDto, UpdateBookingDto, BookingStatsDto
│   ├── Room/                    — RoomDto, CreateRoomDto, UpdateRoomDto
│   ├── Hotel/                   — HotelSettingDto, UpdateHotelSettingDto
│   └── Common/                  — ApiResponse<T>
│
├── Data/
│   └── ApplicationDbContext.cs  — EF Core DbContext + model config
│
├── Helpers/
│   └── MappingProfile.cs        — AutoMapper profile
│
├── Migrations/                  — EF Core migrations
└── wwwroot/images/              — uploaded room images
```

---

## 🔌 API endpoints

### Auth
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/auth/login` | ❌ | Login and receive JWT token |

### Rooms
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/rooms` | ❌ | Get all rooms |
| GET | `/api/rooms/{id}` | ❌ | Get room by ID |
| GET | `/api/rooms/available` | ❌ | Get available rooms |
| GET | `/api/rooms/price/{type}` | ❌ | Get price by room type |
| POST | `/api/rooms` | ✅ Admin | Create a new room |
| PUT | `/api/rooms/{id}` | ✅ Admin | Update a room |
| DELETE | `/api/rooms/{id}` | ✅ Admin | Delete a room |

### Bookings
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/bookings` | ✅ | Get all bookings |
| GET | `/api/bookings/{id}` | ✅ | Get booking by ID |
| POST | `/api/bookings` | ❌ | Create a booking (public) |
| PUT | `/api/bookings/{id}` | ✅ | Update booking status/payment |
| DELETE | `/api/bookings/{id}` | ✅ | Delete a booking |
| GET | `/api/bookings/stats` | ✅ | Get booking statistics |
| GET | `/api/bookings/recent/{count}` | ✅ | Get recent N bookings |

### Hotel Settings
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/hotel/settings` | ❌ | Get hotel info & pricing |
| PUT | `/api/hotel/settings` | ✅ Admin | Update hotel settings |

### Upload
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/upload/room-image` | ✅ Admin | Upload room image |

---

## 📦 API response format

Every endpoint returns a consistent wrapper:

```json
{
  "success": true,
  "message": "Success",
  "data": { },
  "statusCode": 200
}
```

---

## ⚙️ Getting started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)
- Visual Studio 2022+ or VS Code

### Installation

```bash
# 1. Clone the repository
git clone https://github.com/AhmedDabish/Hotel.git
cd Hotel
```

```bash
# 2. Update the connection string in appsettings.json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=HotelDB;Trusted_Connection=True;"
}
```

```bash
# 3. Configure JWT in appsettings.json
"Jwt": {
  "Key": "your-super-secret-key-here"
}
```

```bash
# 4. Apply database migrations
dotnet ef database update
```

```bash
# 5. Run the project
dotnet run
```

The API will be available at `https://localhost:5001` and Swagger at `https://localhost:5001/swagger`.

---

## 🗄️ Database schema

```
Users          — id, username (unique), passwordHash, fullName, role, isActive, createdAt, lastLogin
Rooms          — id, type (indexed), name, pricePerNight, capacity, isAvailable (indexed), description, features, imagePath, createdAt
Bookings       — id, customerName, phone, email (indexed), roomType, checkInDate, checkOutDate, numberOfGuests, totalAmount, status (indexed), isPaid, specialRequests, createdAt
HotelSettings  — id, hotelName, address, phone, email, description, deluxePrice, suitePrice, presidentialPrice, offerTitle, offerDetails, updatedAt
```

---

## 🔐 Authentication

Login to receive a JWT token valid for **8 hours**:

```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "yourpassword"
}
```

Use the token in subsequent requests:

```http
Authorization: Bearer <your_token>
```

---

## 📸 Screenshots

> Add screenshots of the admin dashboard and booking flow here!
> `![Dashboard](screenshots/dashboard.png)`

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature`
3. Commit your changes: `git commit -m 'Add some feature'`
4. Push to the branch: `git push origin feature/your-feature`
5. Open a pull request

---














## 📄 License

This project is licensed under the MIT License.

---

Made with ❤️ by Ahmed Dabish
