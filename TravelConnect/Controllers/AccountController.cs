using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelConnect.Data;
using TravelConnect.Models;

namespace TravelConnect.Controllers
{
    public class AccountController : Controller
    {
        private readonly TravelConnectDbContext _context;

        public AccountController(TravelConnectDbContext context)
        {
            _context = context;
        }

        // =========================
        // LOGIN - GET
        // =========================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // =========================
        // LOGIN - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            string email,
            string password)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "Vui lòng nhập email.";
                return View();
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Vui lòng nhập mật khẩu.";
                return View();
            }

            email = email.Trim();

            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Email.ToLower() == email.ToLower()
                    && x.Password == password
                );

            if (user == null)
            {
                ViewBag.Error =
                    "Email hoặc mật khẩu không đúng.";

                return View();
            }

            // =========================
            // LƯU SESSION
            // =========================
            HttpContext.Session.SetInt32(
                "UserId",
                user.Id
            );

            HttpContext.Session.SetString(
                "UserName",
                user.FullName
            );

            HttpContext.Session.SetString(
                "UserEmail",
                user.Email
            );

            HttpContext.Session.SetString(
                "UserRole",
                user.Role
            );

            return RedirectToAction(
                "Index",
                "Home"
            );
        }

        // =========================
        // REGISTER - GET
        // =========================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // =========================
        // REGISTER - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            string fullName,
            string email,
            string password,
            string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                ViewBag.Error =
                    "Vui lòng nhập họ tên.";

                return View();
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error =
                    "Vui lòng nhập email.";

                return View();
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error =
                    "Vui lòng nhập mật khẩu.";

                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error =
                    "Mật khẩu xác nhận không khớp.";

                return View();
            }

            fullName = fullName.Trim();
            email = email.Trim();

            // =========================
            // KIỂM TRA EMAIL
            // =========================
            bool emailExists =
                await _context.Users
                .AnyAsync(x =>
                    x.Email.ToLower() ==
                    email.ToLower()
                );

            if (emailExists)
            {
                ViewBag.Error =
                    "Email này đã được sử dụng.";

                return View();
            }

            // =========================
            // TẠO USER
            // =========================
            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                Password = password,
                Role = "User"
            };

            _context.Users.Add(newUser);

            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        // =========================
        // LOGOUT
        // =========================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Index",
                "Home"
            );
        }
    }
}