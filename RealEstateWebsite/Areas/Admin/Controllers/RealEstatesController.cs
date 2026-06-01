using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateWebsite.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookie")] 
    [Route("Admin/RealEstates")]
    public class RealEstatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        
        public RealEstatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Trang danh sách bài đăng (GET: /Admin/RealEstates)
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            // Lấy danh sách tin, ưu tiên tin VIP lên trước, tin mới đăng lên đầu
            var list = await _context.RealEstates
                .Include(r => r.Category)
                .OrderByDescending(r => r.IsVIP)
                .ThenByDescending(r => r.CreatedDate)
                .ToListAsync();

            return View(list); // Truyền danh sách này ra giao diện hiển thị
        }

        // Chức năng đổi trạng thái VIP (POST: /Admin/RealEstates/ToggleVip/id)
        [HttpPost("ToggleVip/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleVip(int id)
        {
            var item = await _context.RealEstates.FindAsync(id);
            if (item != null)
            {
                item.IsVIP = !item.IsVIP; // Nếu đang VIP thì thành Thường, và ngược lại
                await _context.SaveChangesAsync(); // Lưu  vào DB
            }
            return RedirectToAction(nameof(Index)); // Tải lại trang danh sách
        }

        // Chức năng Xóa bài đăng (POST: /Admin/RealEstates/Delete/id)
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.RealEstates.FindAsync(id);
            if (item != null)
            {
                _context.RealEstates.Remove(item); // Xóa khỏi DB
                await _context.SaveChangesAsync(); // Xác nhận lưu thay đổi
            }
            return RedirectToAction(nameof(Index)); // Tải lại trang danh sách
        }
    }
}