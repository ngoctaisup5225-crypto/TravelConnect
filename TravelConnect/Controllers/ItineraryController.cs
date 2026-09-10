using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelConnect.Data;
using TravelConnect.Models;

namespace TravelConnect.Controllers
{
    public class ItineraryController : Controller
    {
        private readonly TravelConnectDbContext _context;

        public ItineraryController(TravelConnectDbContext context)
        {
            _context = context;
        }


        // ==============================
        // KIỂM TRA ĐĂNG NHẬP
        // ==============================

        private int? GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }


        // ==============================
        // DANH SÁCH LỊCH TRÌNH
        // ==============================

        public async Task<IActionResult> Index()
        {
            int? userId = GetCurrentUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var itineraries = await _context.Itineraries
                .Where(x => x.UserId == userId.Value)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(itineraries);
        }


        // ==============================
        // CHI TIẾT LỊCH TRÌNH
        // ==============================

        public async Task<IActionResult> Details(int id)
        {
            int? userId = GetCurrentUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var itinerary = await _context.Itineraries
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.UserId == userId.Value);

            if (itinerary == null)
            {
                return NotFound();
            }

            var items = await _context.ItineraryItems
                .Where(x => x.ItineraryId == itinerary.Id)
                .OrderBy(x => x.OrderIndex)
                .ToListAsync();

            ViewBag.Items = items;

            return View(itinerary);
        }


        // ==============================
        // TẠO LỊCH TRÌNH - GET
        // ==============================

        [HttpGet]
        public IActionResult Create()
        {
            int? userId = GetCurrentUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }


        // ==============================
        // TẠO LỊCH TRÌNH - POST
        // ==============================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            string title,
            string description,
            DateTime startDate,
            DateTime endDate,
            bool isPublic)
        {
            int? userId = GetCurrentUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }


            // Kiểm tra tên

            if (string.IsNullOrWhiteSpace(title))
            {
                ViewBag.Error =
                    "Vui lòng nhập tên lịch trình.";

                return View();
            }


            // Kiểm tra ngày

            if (endDate.Date < startDate.Date)
            {
                ViewBag.Error =
                    "Ngày kết thúc phải sau hoặc bằng ngày bắt đầu.";

                return View();
            }


            var itinerary = new Itinerary
            {
                UserId = userId.Value,

                Title = title.Trim(),

                Description =
                    description?.Trim() ?? "",

                StartDate = startDate,

                EndDate = endDate,

                CreatedAt = DateTime.Now,

                IsPublic = isPublic,

                NumberOfLocations = 0
            };


            _context.Itineraries.Add(itinerary);

            await _context.SaveChangesAsync();


            TempData["Success"] =
                "Tạo lịch trình thành công.";


            return RedirectToAction(
                "Details",
                new { id = itinerary.Id });
        }


        // ==============================
        // CHỌN ĐỊA ĐIỂM
        // ==============================

        [HttpGet]
        public async Task<IActionResult> SelectLocation(
            int itineraryId)
        {
            int? userId = GetCurrentUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var itinerary = await _context.Itineraries
                .FirstOrDefaultAsync(x =>
                    x.Id == itineraryId &&
                    x.UserId == userId.Value);

            if (itinerary == null)
            {
                return NotFound();
            }


            var locations = await _context.Locations
                .OrderBy(x => x.Id)
                .ToListAsync();


            ViewBag.Itinerary = itinerary;

            return View(locations);
        }


        // ==============================
        // FORM THÊM ĐỊA ĐIỂM
        // ==============================

        [HttpGet]
        public async Task<IActionResult> AddLocationForm(
            int itineraryId,
            int locationId)
        {
            int? userId = GetCurrentUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var itinerary = await _context.Itineraries
                .FirstOrDefaultAsync(x =>
                    x.Id == itineraryId &&
                    x.UserId == userId.Value);

            if (itinerary == null)
            {
                return NotFound();
            }


            var location = await _context.Locations
                .FirstOrDefaultAsync(x =>
                    x.Id == locationId);

            if (location == null)
            {
                return NotFound();
            }


            ViewBag.Itinerary = itinerary;

            ViewBag.Location = location;


            return View();
        }


        // ==============================
        // THÊM ĐỊA ĐIỂM
        // ==============================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLocation(
            int itineraryId,
            int locationId,
            string day,
            string time,
            string note)
        {
            int? userId = GetCurrentUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var itinerary = await _context.Itineraries
                .FirstOrDefaultAsync(x =>
                    x.Id == itineraryId &&
                    x.UserId == userId.Value);

            if (itinerary == null)
            {
                return NotFound();
            }


            var location = await _context.Locations
                .FirstOrDefaultAsync(x =>
                    x.Id == locationId);

            if (location == null)
            {
                return NotFound();
            }


            // Kiểm tra ngày

            if (string.IsNullOrWhiteSpace(day))
            {
                TempData["Error"] =
                    "Vui lòng chọn ngày.";

                return RedirectToAction(
                    "AddLocationForm",
                    new
                    {
                        itineraryId,
                        locationId
                    });
            }


            // Không cho thêm trùng địa điểm

            bool exists = await _context.ItineraryItems
                .AnyAsync(x =>
                    x.ItineraryId == itineraryId &&
                    x.LocationId == locationId);

            if (exists)
            {
                TempData["Error"] =
                    "Địa điểm này đã có trong lịch trình.";

                return RedirectToAction(
                    "SelectLocation",
                    new { itineraryId });
            }


            // Tìm OrderIndex tiếp theo

            int nextOrder =
                await _context.ItineraryItems
                    .Where(x =>
                        x.ItineraryId == itineraryId)
                    .Select(x => (int?)x.OrderIndex)
                    .MaxAsync() ?? 0;

            nextOrder++;


            var item = new ItineraryItem
            {
                ItineraryId = itineraryId,

                LocationId = locationId,

                LocationName = location.Name,

                Day = day.Trim(),

                Time = time?.Trim() ?? "",

                Note = note?.Trim() ?? "",

                OrderIndex = nextOrder
            };


            _context.ItineraryItems.Add(item);


            itinerary.NumberOfLocations++;


            await _context.SaveChangesAsync();


            TempData["Success"] =
                $"Đã thêm {location.Name} vào lịch trình.";


            return RedirectToAction(
                "Details",
                new { id = itineraryId });
        }


        // ==============================
        // XÓA ĐỊA ĐIỂM KHỎI LỊCH TRÌNH
        // ==============================

        [HttpGet]
        public async Task<IActionResult> RemoveLocation(
            int id,
            int itineraryId)
        {
            int? userId = GetCurrentUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var itinerary = await _context.Itineraries
                .FirstOrDefaultAsync(x =>
                    x.Id == itineraryId &&
                    x.UserId == userId.Value);

            if (itinerary == null)
            {
                return NotFound();
            }


            var item = await _context.ItineraryItems
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.ItineraryId == itineraryId);

            if (item != null)
            {
                _context.ItineraryItems.Remove(item);


                if (itinerary.NumberOfLocations > 0)
                {
                    itinerary.NumberOfLocations--;
                }


                await _context.SaveChangesAsync();


                TempData["Success"] =
                    "Đã xóa địa điểm khỏi lịch trình.";
            }


            return RedirectToAction(
                "Details",
                new { id = itineraryId });
        }


        // ==============================
        // XÓA LỊCH TRÌNH
        // ==============================

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            int? userId = GetCurrentUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var itinerary = await _context.Itineraries
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.UserId == userId.Value);

            if (itinerary == null)
            {
                return NotFound();
            }


            var items = await _context.ItineraryItems
                .Where(x => x.ItineraryId == id)
                .ToListAsync();


            if (items.Any())
            {
                _context.ItineraryItems.RemoveRange(items);
            }


            _context.Itineraries.Remove(itinerary);


            await _context.SaveChangesAsync();


            TempData["Success"] =
                "Đã xóa lịch trình.";


            return RedirectToAction("Index");
        }


        // ==============================
        // CHỈNH SỬA - GET
        // ==============================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            int? userId = GetCurrentUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var itinerary = await _context.Itineraries
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.UserId == userId.Value);

            if (itinerary == null)
            {
                return NotFound();
            }


            return View(itinerary);
        }


        // ==============================
        // CHỈNH SỬA - POST
        // ==============================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            string title,
            string description,
            DateTime startDate,
            DateTime endDate,
            bool isPublic)
        {
            int? userId = GetCurrentUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }


            var itinerary = await _context.Itineraries
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.UserId == userId.Value);

            if (itinerary == null)
            {
                return NotFound();
            }


            // Kiểm tra tên

            if (string.IsNullOrWhiteSpace(title))
            {
                ViewBag.Error =
                    "Vui lòng nhập tên lịch trình.";

                return View(itinerary);
            }


            // Kiểm tra ngày

            if (endDate.Date < startDate.Date)
            {
                ViewBag.Error =
                    "Ngày kết thúc phải sau hoặc bằng ngày bắt đầu.";

                return View(itinerary);
            }


            // Cập nhật

            itinerary.Title =
                title.Trim();

            itinerary.Description =
                description?.Trim() ?? "";

            itinerary.StartDate =
                startDate;

            itinerary.EndDate =
                endDate;

            itinerary.IsPublic =
                isPublic;


            await _context.SaveChangesAsync();


            TempData["Success"] =
                "Đã cập nhật lịch trình thành công.";


            return RedirectToAction(
                "Details",
                new { id = itinerary.Id });
        }
    }
}