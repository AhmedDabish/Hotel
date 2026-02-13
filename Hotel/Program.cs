using Hotel.Data;
using Hotel.Helpers;
using Hotel.Models;
using Hotel.Repositories;
using Hotel.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine($"Connection String: {connectionString}");
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IHotelSettingRepository, HotelSettingRepository>();

// Add Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IHotelService, HotelService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("JWT Key is not configured");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userRepository = services.GetRequiredService<IUserRepository>();

        // تأكد من إنشاء قاعدة البيانات
        Console.WriteLine("Applying migrations...");
        context.Database.EnsureCreated();
        Console.WriteLine("Migrations applied successfully!");

        // التحقق من اتصال قاعدة البيانات
        var canConnect = await context.Database.CanConnectAsync();
        Console.WriteLine($"Can connect to database: {canConnect}");

        // إضافة المستخدم الافتراضي إذا لم يكن موجوداً
        if (!context.Users.Any())
        {
            Console.WriteLine("Creating default admin user...");
            var adminUser = new Hotel.Models.User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                FullName = "مالك الفندق",
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await userRepository.AddAsync(adminUser);
            Console.WriteLine("Default admin user created successfully!");
        }

        // إضافة إعدادات افتراضية للفندق
        if (!context.HotelSettings.Any())
        {
            Console.WriteLine("Creating default hotel settings...");
            var defaultSettings = new Hotel.Models.HotelSetting
            {
                HotelName = "فندق الرفاهية",
                Address = "شارع الرفاهية، وسط المدينة",
                Phone = "+966 12 345 6789",
                Email = "info@luxuryhotel.com",
                Description = "فندق الرفاهية يوفر لك تجربة إقامة استثنائية تجمع بين الفخامة والراحة والخدمات المتميزة في قلب المدينة",
                DeluxePrice = 120,
                SuitePrice = 250,
                PresidentialPrice = 500,
                OfferTitle = "خصم 20% للحجز المبكر",
                OfferDetails = "احصل على خصم 20% عند الحجز قبل 30 يوم من تاريخ الوصول",
                UpdatedAt = DateTime.UtcNow
            };

            await context.HotelSettings.AddAsync(defaultSettings);
            await context.SaveChangesAsync();
            Console.WriteLine("Default hotel settings created successfully!");
        }

        // إضافة غرف افتراضية
        if (!context.Rooms.Any())
        {
            Console.WriteLine("Creating default rooms...");
            var defaultRooms = new List<Room>
            {
                new Room
                {
                    Type = "Deluxe",
                    Name = "غرفة ديلوكس",
                    PricePerNight = 120,
                    Capacity = 2,
                    IsAvailable = true,
                    Description = "غرفة فسيحة مع إطلالة رائعة على المدينة، مجهزة بكل وسائل الراحة الحديثة",
                    Features = "واي فاي مجاني, تلفزيون بشاشة مسطحة, ميني بار, مكيف هواء",
                    CreatedAt = DateTime.UtcNow
                },
                new Room
                {
                    Type = "Suite",
                    Name = "جناح فاخر",
                    PricePerNight = 250,
                    Capacity = 4,
                    IsAvailable = true,
                    Description = "جناح فاخر يتكون من غرفة معيشة وغرفة نوم منفصلة وحمام فخم مع جاكوزي",
                    Features = "واي فاي مجاني, تلفزيون بشاشة مسطحة 55 بوصة, ميني بار فاخر, جاكوزي, إطلالة على المدينة",
                    CreatedAt = DateTime.UtcNow
                },
                new Room
                {
                    Type = "Presidential",
                    Name = "الجناح الرئاسي",
                    PricePerNight = 500,
                    Capacity = 6,
                    IsAvailable = true,
                    Description = "جناح رئاسي فاخر مع صالة استقبال ومطبخ خاص وإطلالة بانورامية على المدينة",
                    Features = "واي فاي مجاني, تلفزيون بشاشة مسطحة 65 بوصة, ميني بار فاخر, جاكوزي, مطبخ خاص, صالة استقبال, إطلالة بانورامية",
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Rooms.AddRangeAsync(defaultRooms);
            await context.SaveChangesAsync();
            Console.WriteLine("Default rooms created successfully!");
        }

        Console.WriteLine("Database seeding completed successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");
    }
}

Console.WriteLine($"API is running on: {app.Urls}");
app.Run();