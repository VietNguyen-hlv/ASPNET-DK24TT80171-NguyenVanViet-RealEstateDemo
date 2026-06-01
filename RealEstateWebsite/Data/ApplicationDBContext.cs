using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using RealEstateWebsite.Models; // ⚠️ CHÚ Ý: Thay "YourProjectName" bằng tên dự án thật của bạn

namespace RealEstateWebsite.Data // ⚠️ CHÚ Ý: Thay "YourProjectName" bằng tên dự án thật của bạn
{
    public class ApplicationDbContext : DbContext
    {
        // Hàm khởi tạo nhận cấu hình kết nối từ file Program.cs và truyền vào lớp cha (DbContext)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ============================================================
        // ĐĂNG KÝ CÁC CLASS MODEL THÀNH CÁC BẢNG TRONG DATABASE
        // ============================================================

        // 1. Kích hoạt bảng Danh mục (Categories) dưới SQL Server
        public DbSet<Category> Categories { get; set; }

        // 2. Kích hoạt bảng Tin đăng Bất động sản (RealEstates) dưới SQL Server
        public DbSet<RealEstate> RealEstates { get; set; }
        public DbSet<User> Users { get; set; }
    }
}