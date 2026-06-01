using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;

namespace RealEstateWebsite.Models // Nhớ thay "YourProjectName" bằng tên dự án thật của bạn
{
    [Table("Categories")] // Khai báo cho Entity Framework biết Class này đại diện cho bảng "Categories" trong SQL
    public class Category
    {
        [Key] // Đánh dấu đây là Khóa chính (Primary Key)
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")] // Ràng buộc bắt buộc nhập (NOT NULL)
        [StringLength(100)] // Giới hạn tối đa 100 ký tự tương ứng NVARCHAR(100)
        [Display(Name = "Tên danh mục")] // Nhãn hiển thị trên giao diện người dùng
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Slug { get; set; } // Dùng để làm URL thân thiện, ví dụ: "can-ho-chung-cu"

        // ---- MỐI QUAN HỆ TRONG ERD ----
        // Quan hệ 1 - Nhiều: 1 Danh mục có thể chứa nhiều tin đăng Bất động sản khác nhau
        public virtual ICollection<RealEstate> RealEstates { get; set; } = new List<RealEstate>();
    }
}
