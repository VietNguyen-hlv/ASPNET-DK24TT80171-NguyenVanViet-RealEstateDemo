using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")] 
    public class AccountController : Controller
    {
        
        [HttpGet("login")]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
        {
            
            if (username == "admin" && password == "admin@123")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, "AdminCookie");

                
                await HttpContext.SignInAsync("AdminCookie", new ClaimsPrincipal(claimsIdentity));

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            ModelState.AddModelError("", "Tài khoản hoặc mật khẩu Quản trị không chính xác!");
            return View();
        }

       
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AdminCookie");
            return RedirectToAction("Login", "Account", new { area = "Admin" });
        }
    }
}