using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelConnect.Data;
using TravelConnect.Models;

namespace TravelConnect.Controllers
{
    public class AIController : Controller
    {
        private readonly TravelConnectDbContext _context;

        public AIController(TravelConnectDbContext context)
        {
            _context = context;
        }


        // ==============================
        // TRANG AI
        // ==============================

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        // ==============================
        // GỢI Ý ĐỊA ĐIỂM
        // ==============================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recommend(
            string destination,
            int days,
            decimal budget,
            string interest)
        {
            // Kiểm tra số ngày

            if (days <= 0)
            {
                ViewBag.Error =
                    "Vui lòng nhập số ngày hợp lệ.";

                return View("Index");
            }


            // Kiểm tra ngân sách

            if (budget < 0)
            {
                ViewBag.Error =
                    "Ngân sách không hợp lệ.";

                return View("Index");
            }


            // Lấy toàn bộ địa điểm

            var locations = await _context.Locations
                .ToListAsync();


            if (!locations.Any())
            {
                ViewBag.Error =
                    "Hệ thống chưa có địa điểm để gợi ý.";

                return View("Index");
            }


            // Chuẩn hóa từ khóa

            destination =
                destination?.Trim() ?? "";

            interest =
                interest?.Trim() ?? "";


            // ==============================
            // LỌC ĐỊA ĐIỂM
            // ==============================

            var recommendations =
                locations.AsEnumerable();


            // Nếu có tỉnh/thành phố

            if (!string.IsNullOrWhiteSpace(destination))
            {
                var destinationResults =
                    recommendations
                    .Where(x =>
                        x.Province.Contains(
                            destination,
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        x.Name.Contains(
                            destination,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (destinationResults.Any())
                {
                    recommendations =
                        destinationResults;
                }
            }


            // Nếu có sở thích

            if (!string.IsNullOrWhiteSpace(interest))
            {
                var interestResults =
                    recommendations
                    .Where(x =>
                        x.Category.Contains(
                            interest,
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        x.Name.Contains(
                            interest,
                            StringComparison.OrdinalIgnoreCase)
                        ||
                        x.Description.Contains(
                            interest,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (interestResults.Any())
                {
                    recommendations =
                        interestResults;
                }
            }


            // ==============================
            // XẾP HẠNG
            // ==============================

            var result =
                recommendations
                .OrderByDescending(x => x.Rating)
                .Take(6)
                .ToList();


            // Nếu lọc quá chặt

            if (!result.Any())
            {
                result =
                    locations
                    .OrderByDescending(x => x.Rating)
                    .Take(6)
                    .ToList();
            }


            // ==============================
            // TẠO GỢI Ý LỊCH TRÌNH
            // ==============================

            var itinerarySuggestions =
                new List<TravelConnect.Models.Location>();


            for (int i = 0;
                 i < days && i < result.Count;
                 i++)
            {
                itinerarySuggestions.Add(result[i]);
            }


            ViewBag.Destination =
                destination;

            ViewBag.Days =
                days;

            ViewBag.Budget =
                budget;

            ViewBag.Interest =
                interest;

            ViewBag.Recommendations =
                result;

            ViewBag.ItinerarySuggestions =
                itinerarySuggestions;


            return View("Result");
        }
    }
}