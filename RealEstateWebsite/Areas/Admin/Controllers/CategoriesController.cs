using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateWebsite.Data;
using RealEstateWebsite.Models;
using System.Threading.Tasks;

namespace RealEstateWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookie")]
    [Route("Admin/Categories")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }
                
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var list = await _context.Categories.ToListAsync();
            return View(list);
        }
               
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RealEstateWebsite.Models.Category Category)
        {
            ModelState.Remove("Slug");
            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(Category.Name))
                {
                    Category.Slug = GenerateSlug(Category.Name);
                }
                                                
                _context.Categories.Add(Category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
            }
            return RedirectToAction(nameof(Index));
                        
        }
                        
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    
       private string GenerateSlug(string title)
        {
            if (string.IsNullOrEmpty(title)) return "";
            var slug = title.ToLower().Trim();
            string[] convert = {
        "aàảãáạăằẳẵắặâầẩẫấậ", "dđ", "eèẻẽéẹêềểễếệ", "iìỉĩíị",
        "oòỏõóọôồổỗốộơờởỡớợ", "uùủũúụưừửữứự", "yỳỷỹýỵ"
    };
            for (int i = 0; i < convert.Length; i++)
            {
                char replaceChar = convert[i][0];
                for (int j = 1; j < convert[i].Length; j++)
                    slug = slug.Replace(convert[i][j], replaceChar);
            }
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-").Trim();
            return slug;
        }
    }
}
