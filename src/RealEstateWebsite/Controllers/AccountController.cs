using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateWebsite.Data;
using RealEstateWebsite.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RealEstateWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
                        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
       public IActionResult Login(string email, string user , string password)
        {
            
            if (email == "admin@gmail.com" && password == "123456") // Giả lập tài khoản admin để chạy thử nghiệm giao diện
            {
                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Email, email),
                 new Claim(ClaimTypes.Name, "Admin User")
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                                              
                return RedirectToAction("Index", "batdongsan"); // Đăng nhập đúng điều hướng về trang chủ
            }

            //if (user == null || !user.IsActive)
            //{
             //   ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng!");
              //  return View();
            //}

            // --- ĐOẠN CẦN SỬA CHÍNH XÁC ---
           // var claims = new List<Claim>
            //  {
        // Dòng này bắt buộc phải là ClaimTypes.Name để @Context.User.Identity.Name nhận diện được tên
                //new Claim(ClaimTypes.Name, user.FullName ?? user.Username ?? "Thành viên"),
                // new Claim(ClaimTypes.Email, user.Email ?? ""),
                //new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "Member")
                // };
            ViewBag.ErrorMessage = "Email hoặc mật khẩu không chính xác!";
            return View();
        }

       
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string FullName, string Email, string Username,  string PhoneNumber, string Password)
        {
           
            if (string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin bắt buộc!");
                return View();
            }

            var exists = await _context.Users.AnyAsync(u => u.Email == Email.Trim());
            if (exists)
            {
                ModelState.AddModelError("", "Email này đã được sử dụng trên hệ thống!");
                return View();
            }
                     
            var newUser = new RealEstateWebsite.Models.User
            {
                FullName = FullName.Trim(),
                Email = Email.Trim(),
                Username = Username.Trim(),
                PasswordHash = Password.Trim(),
                RoleId = 2,
                IsActive = true
            };
                        
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
                       
            return RedirectToAction("Login", "Account");
        }
                       
    }
}