using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateWebsite.Data; 
using RealEstateWebsite.Models;
using System.Linq;
using System.Threading.Tasks;


namespace RealEstateWebsite.Controllers
{
    
    public class batdongsanController : Controller
    {
        private readonly ApplicationDbContext _context;
                
        public batdongsanController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string type)
        {
            // Tạo câu lệnh truy vấn gốc kèm bảng danh mục Category
            var query = _context.RealEstates.Include(r => r.Category).AsQueryable();

            // Tiến hành lọc dữ liệu dựa trên nút người dùng bấm
            if (!string.IsNullOrEmpty(type))
            {
                type = type.ToLower();

                if (type == "ban")
                {
                    query = query.Where(r => r.Category.Name.Contains("Bán") || r.Title.Contains("Bán"));
                    ViewData["CurrentFilter"] = "🏠 DANH SÁCH NHÀ ĐẤT ĐANG BÁN";
                }
                else if (type == "thue")
                {
                    query = query.Where(r => r.Category.Name.Contains("Thuê") || r.Title.Contains("Thuê"));
                    ViewData["CurrentFilter"] = "🔑 DANH SÁCH BẤT ĐỘNG SẢN CHO THUÊ";
                }
                else if (type == "canmua")
                {
                    query = query.Where(r => r.Category.Name.Contains("Mua") || r.Title.Contains("Cần Mua"));
                    ViewData["CurrentFilter"] = "🔍 TIN CẦN MUA - CẦN THUÊ KHÁCH HÀNG TÌM KIẾM";
                }
            }
            else
            {
                ViewData["CurrentFilter"] = "✨ TẤT CẢ TIN ĐĂNG BẤT ĐỘNG SẢN NỔI BẬT";
            }

            // Ưu Tiên Sắp xếp tin VIP lên trước, tin mới nhất lên đầu
            var result = await query.OrderByDescending(r => r.IsVIP).ThenByDescending(r => r.CreatedDate).ToListAsync();

            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> Create(string formType)
        {
            
            ViewBag.CategoryId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Categories.ToListAsync(), "Id", "Name");

            // Kiểm tra Loại form nào Người dùng bấm để truyền tiêu đề động ra View
            if (formType == "thue")
            {
                ViewData["FormTitle"] = "ĐĂNG TIN CHO THUÊ BẤT ĐỘNG SẢN";
                ViewData["FormColor"] = "bg-info text-white"; // Màu xanh lam tươi mát cho phân hệ Thuê
            }
            else if (formType == "canmua")
            {
                ViewData["FormTitle"] = "ĐĂNG TIN CẦN MUA - TÌM KIẾM KHÁCH HÀNG";
                ViewData["FormColor"] = "bg-success text-white"; // Màu xanh lá cho phân hệ Cần Mua
            }
            else
            {
                ViewData["FormTitle"] = "ĐĂNG TIN RAO BÁN NHÀ ĐẤT";
                ViewData["FormColor"] = "bg-primary text-white"; // Màu xanh dương mặc định cho Bán
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Price,Area,AddressNumber,Direction,CategoryId,IsVIP")] RealEstate realEstate, IFormFile ImageFile)
        {
            realEstate.UserId = 1;
            realEstate.WardId = 1;
            realEstate.Status = 1;
            realEstate.CreatedDate = DateTime.Now;

           
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                                
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }
                                
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(imagesFolder, uniqueFileName);
                                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                                
                realEstate.ImageUrl = uniqueFileName;
            }
            else
            {
                realEstate.ImageUrl = "default-house.jpg";
            }
            ModelState.Remove("Category");
            ModelState.Remove("UserId");
            ModelState.Remove("WardId");
            ModelState.Remove("Status");
            ModelState.Remove("ImageUrl");
            if (ModelState.IsValid)
            {
                _context.Add(realEstate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Categories.ToListAsync(), "Id", "Name", realEstate.CategoryId);
            return View(realEstate);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); 
            }
                       
            var realEstate = await _context.RealEstates
                                            .Include(r => r.Category)
                                            .FirstOrDefaultAsync(m => m.Id == id);

            if (realEstate == null)
            {
                return NotFound(); 
            }

            return View(realEstate); 
        }
        //  CHỨC NĂNG CHỈNH SỬA (EDIT)
        // Hiển thị Form Edit với dữ liệu cũ (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var realEstate = await _context.RealEstates.FindAsync(id);
            if (realEstate == null) return NotFound();

            ViewBag.CategoryId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Categories.ToListAsync(), "Id", "Name", realEstate.CategoryId);
            return View(realEstate);
        }

        // Nhận dữ liệu sửa đổi và cập nhật xuống DB (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Price,Area,AddressNumber,Direction,CategoryId,IsVIP,UserId,WardId,Status,CreatedDate")] RealEstate realEstate)
        {
           
            if (id != realEstate.Id) return NotFound();
                       
            if (ModelState.IsValid)
           
            {
                try
                {
                    _context.Update(realEstate);
                    await _context.SaveChangesAsync();
                                 }
                catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
                {
                    if (!_context.RealEstates.Any(e => e.Id == realEstate.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Categories.ToListAsync(), "Id", "Name", realEstate.CategoryId);
            return View(realEstate);
        }

        //  CHỨC NĂNG XÓA (DELETE)
        // Hàm xóa nhanh bài đăng thẳng từ trang danh sách (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var realEstate = await _context.RealEstates.FindAsync(id);
            if (realEstate != null)
            {
                _context.RealEstates.Remove(realEstate);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
      
    }

}

    
    
