using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelConnect.Data;
using TravelConnect.Models;

namespace TravelConnect.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly TravelConnectDbContext _context;

        public FavoriteController(TravelConnectDbContext context)
        {
            _context = context;
        }

        // =========================
        // DANH SÁCH YÊU THÍCH
        // =========================
        public async Task<IActionResult> Index()
        {
            int? userId =
                HttpContext.Session.GetInt32("UserId");

            // Chưa đăng nhập
            if (userId == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account"
                );
            }

            var favorites =
                await _context.Favorites
                .Where(x => x.UserId == userId.Value)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return View(favorites);
        }

        // =========================
        // THÊM YÊU THÍCH
        // =========================
        [HttpGet]
        public async Task<IActionResult> Add(int id)
        {
            int? userId =
                HttpContext.Session.GetInt32("UserId");

            // Chưa đăng nhập
            if (userId == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account"
                );
            }

            // Tìm địa điểm trong SQL
            var location =
                await _context.Locations
                .FirstOrDefaultAsync(x => x.Id == id);

            if (location == null)
            {
                return NotFound();
            }

            // Kiểm tra đã lưu chưa
            bool exists =
                await _context.Favorites
                .AnyAsync(x =>
                    x.UserId == userId.Value &&
                    x.LocationId == location.Id
                );

            // Nếu chưa lưu thì thêm
            if (!exists)
            {
                var favorite = new Favorite
                {
                    UserId = userId.Value,

                    LocationId = location.Id,

                    LocationName = location.Name,

                    Province = location.Province,

                    Image = location.Image,

                    Rating = location.Rating
                };

                _context.Favorites.Add(favorite);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // =========================
        // XÓA YÊU THÍCH
        // =========================
        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            int? userId =
                HttpContext.Session.GetInt32("UserId");

            // Chưa đăng nhập
            if (userId == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account"
                );
            }

            // Chỉ được xóa yêu thích
            // của chính mình
            var favorite =
                await _context.Favorites
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.UserId == userId.Value
                );

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}