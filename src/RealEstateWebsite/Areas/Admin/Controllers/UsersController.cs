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
    [Route("Admin/Users")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Show danh sách thành viên (GET: /Admin/Users)
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var list = await _context.Users.OrderByDescending(u => u.Id).ToListAsync();
            return View(list);
        }

        // Quản lý trạng thái hoạt động tài khoản
        [HttpPost("ToggleStatus/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsActive = !user.IsActive; 
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}