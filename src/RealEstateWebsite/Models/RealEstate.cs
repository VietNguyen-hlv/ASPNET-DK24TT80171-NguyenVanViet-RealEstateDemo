using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RealEstateWebsite.Models
{
    [Table("RealEstates")] 
    public class RealEstate
    {
        [Key] 
        public int Id { get; set; }

        [Required(ErrorMessage = "Tiêu đề tin đăng là bắt buộc")]
        [StringLength(255, ErrorMessage = "Tiêu đề không được vượt quá 255 ký tự")]
        [Display(Name = "Tiêu đề tin")]
        public string Title { get; set; }

        [Display(Name = "Mô tả chi tiết")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Giá bán là bắt buộc")]
        [Column(TypeName = "decimal(18,2)")] 
        [Display(Name = "Giá (VNĐ)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Diện tích là bắt buộc")]
        [Display(Name = "Diện tích (m²)")]
        public double Area { get; set; }

        [StringLength(255)]
        [Display(Name = "Số nhà, tên đường")]
        public string AddressNumber { get; set; }

      

        [Required]
        public int WardId { get; set; } 

        [Required]
        
        public int CategoryId { get; set; }

        [Required]
        public int UserId { get; set; } 

  
        public int Status { get; set; } = 0; // 0: Chờ duyệt, 1: Đã duyệt, 2: Đã ẩn

        [Display(Name = "Tin VIP")]
        public bool IsVIP { get; set; } = false; // Đánh dấu tin nổi bật có trả phí hay không

        [StringLength(50)]
        [Display(Name = "Hướng nhà")]
        public string Direction { get; set; } 

        [Display(Name = "Ngày đăng")]
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Tự động lấy thời gian hiện tại lúc tạo tin

        public string? ImageUrl { get; set; }

        [ForeignKey("CategoryId")] 
        public virtual Category Category { get; set; }
    }
}