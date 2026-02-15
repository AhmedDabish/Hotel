using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Hotel.DTOs.Common;

namespace Hotel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _wwwRootPath;

        public UploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
            _wwwRootPath = Path.Combine(_environment.WebRootPath ?? _environment.ContentRootPath, "wwwroot", "images");

            // لو المجلد مش موجود، اعمله
            if (!Directory.Exists(_wwwRootPath))
            {
                Directory.CreateDirectory(_wwwRootPath);
            }
        }

        [HttpPost("room-image")]
        public async Task<IActionResult> UploadRoomImage(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return Ok(new { success = false, message = "الملف غير صالح" });
                }

                // التحقق من نوع الملف
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".avif" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return Ok(new { success = false, message = "نوع الملف غير مسموح به" });
                }

                // اسم فريد للملف
                var fileName = Guid.NewGuid().ToString() + fileExtension;

                // المسار الصحيح للمجلد wwwroot/images
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                // لو المجلد مش موجود، اعمله
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, fileName);

                // حفظ الملف
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // المسار اللي هيتحفظ في قاعدة البيانات
                var imagePath = "/images/" + fileName;

                return Ok(new
                {
                    success = true,
                    data = imagePath,
                    message = "تم رفع الصورة بنجاح"
                });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = "حدث خطأ: " + ex.Message });
            }
        }
    }
}