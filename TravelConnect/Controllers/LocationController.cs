using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelConnect.Data;
using TravelConnect.Models;

namespace TravelConnect.Controllers
{
    public class LocationController : Controller
    {
        private readonly TravelConnectDbContext _context;

        public LocationController(TravelConnectDbContext context)
        {
            _context = context;
        }


        // =====================================================
        // DANH SÁCH ĐỊA ĐIỂM
        // =====================================================

        public async Task<IActionResult> Index(
            string? search,
            string? category)
        {
            var query = _context.Locations
                .AsQueryable();


            // Tìm kiếm
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(x =>
                    x.Name.Contains(search) ||
                    x.Province.Contains(search)
                );
            }


            // Lọc danh mục
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(x =>
                    x.Category == category
                );
            }


            var locations = await query
                .OrderBy(x => x.Id)
                .ToListAsync();


            ViewBag.Search = search;
            ViewBag.Category = category;


            return View(locations);
        }


        // =====================================================
        // CHI TIẾT ĐỊA ĐIỂM
        // =====================================================

        public async Task<IActionResult> Details(int id)
        {
            var location =
                await _context.Locations
                .FirstOrDefaultAsync(x => x.Id == id);


            if (location == null)
            {
                return NotFound();
            }


            return View(location);
        }


        // =====================================================
        // KIỂM TRA ADMIN
        // =====================================================

        private bool IsAdmin()
        {
            return
                HttpContext.Session.GetString("UserRole")
                == "Admin";
        }


        private IActionResult? CheckAdmin()
        {
            int? userId =
                HttpContext.Session.GetInt32("UserId");


            if (userId == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account"
                );
            }


            if (!IsAdmin())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            return null;
        }


        // =====================================================
        // ADMIN - DANH SÁCH ĐỊA ĐIỂM
        // =====================================================

        public async Task<IActionResult> AdminIndex()
        {
            var check = CheckAdmin();

            if (check != null)
            {
                return check;
            }


            var locations =
                await _context.Locations
                .OrderBy(x => x.Id)
                .ToListAsync();


            return View(locations);
        }


        // =====================================================
        // ADMIN - CREATE GET
        // =====================================================

        [HttpGet]
        public IActionResult Create()
        {
            var check = CheckAdmin();

            if (check != null)
            {
                return check;
            }


            return View();
        }


        // =====================================================
        // ADMIN - CREATE POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Location location)
        {
            var check = CheckAdmin();

            if (check != null)
            {
                return check;
            }


            if (string.IsNullOrWhiteSpace(location.Name))
            {
                ModelState.AddModelError(
                    "Name",
                    "Vui lòng nhập tên địa điểm."
                );
            }


            if (string.IsNullOrWhiteSpace(location.Province))
            {
                ModelState.AddModelError(
                    "Province",
                    "Vui lòng nhập tỉnh/thành phố."
                );
            }


            if (string.IsNullOrWhiteSpace(location.Category))
            {
                ModelState.AddModelError(
                    "Category",
                    "Vui lòng chọn danh mục."
                );
            }


            if (!ModelState.IsValid)
            {
                return View(location);
            }


            // Không tự nhập Id
            location.Id = 0;


            _context.Locations.Add(location);


            await _context.SaveChangesAsync();


            return RedirectToAction(
                "AdminIndex"
            );
        }


        // =====================================================
        // ADMIN - EDIT GET
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var check = CheckAdmin();

            if (check != null)
            {
                return check;
            }


            var location =
                await _context.Locations
                .FirstOrDefaultAsync(x => x.Id == id);


            if (location == null)
            {
                return NotFound();
            }


            return View(location);
        }


        // =====================================================
        // ADMIN - EDIT POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Location location)
        {
            var check = CheckAdmin();

            if (check != null)
            {
                return check;
            }


            if (id != location.Id)
            {
                return NotFound();
            }


            if (string.IsNullOrWhiteSpace(location.Name))
            {
                ModelState.AddModelError(
                    "Name",
                    "Vui lòng nhập tên địa điểm."
                );
            }


            if (string.IsNullOrWhiteSpace(location.Province))
            {
                ModelState.AddModelError(
                    "Province",
                    "Vui lòng nhập tỉnh/thành phố."
                );
            }


            if (string.IsNullOrWhiteSpace(location.Category))
            {
                ModelState.AddModelError(
                    "Category",
                    "Vui lòng chọn danh mục."
                );
            }


            if (!ModelState.IsValid)
            {
                return View(location);
            }


            var existingLocation =
                await _context.Locations
                .FirstOrDefaultAsync(x => x.Id == id);


            if (existingLocation == null)
            {
                return NotFound();
            }


            existingLocation.Name =
                location.Name.Trim();

            existingLocation.Province =
                location.Province.Trim();

            existingLocation.Category =
                location.Category.Trim();

            existingLocation.Description =
                location.Description ?? "";

            existingLocation.Image =
                location.Image ?? "";

            existingLocation.Rating =
                location.Rating;


            await _context.SaveChangesAsync();


            return RedirectToAction(
                "AdminIndex"
            );
        }


        // =====================================================
        // ADMIN - DELETE
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var check = CheckAdmin();

            if (check != null)
            {
                return check;
            }


            var location =
                await _context.Locations
                .FirstOrDefaultAsync(x => x.Id == id);


            if (location != null)
            {
                _context.Locations.Remove(
                    location
                );

                await _context.SaveChangesAsync();
            }


            return RedirectToAction(
                "AdminIndex"
            );
        }
    }
}