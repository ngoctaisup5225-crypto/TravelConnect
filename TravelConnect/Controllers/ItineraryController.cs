using Microsoft.AspNetCore.Mvc;
using TravelConnect.Models;

namespace TravelConnect.Controllers
{
    public class ItineraryController : Controller
    {
        // ==============================
        // DANH SÁCH LỊCH TRÌNH
        // ==============================

        private static List<Itinerary> itineraries = new List<Itinerary>
        {
            new Itinerary
            {
                Id = 1,
                UserId = 1,
                Title = "Khám phá Đà Nẵng 3 ngày 2 đêm",
                Description = "Lịch trình khám phá những địa điểm nổi bật tại Đà Nẵng.",
                StartDate = new DateTime(2026, 9, 10),
                EndDate = new DateTime(2026, 9, 12),
                CreatedAt = new DateTime(2026, 8, 25),
                IsPublic = true,
                NumberOfLocations = 7
            },

            new Itinerary
            {
                Id = 2,
                UserId = 1,
                Title = "Đà Lạt chill 2 ngày",
                Description = "Một chuyến đi ngắn để nghỉ ngơi và khám phá Đà Lạt.",
                StartDate = new DateTime(2026, 10, 5),
                EndDate = new DateTime(2026, 10, 6),
                CreatedAt = new DateTime(2026, 8, 28),
                IsPublic = true,
                NumberOfLocations = 5
            },

            new Itinerary
            {
                Id = 3,
                UserId = 1,
                Title = "Phú Quốc 4 ngày 3 đêm",
                Description = "Khám phá biển đảo và những địa điểm nổi tiếng ở Phú Quốc.",
                StartDate = new DateTime(2026, 11, 1),
                EndDate = new DateTime(2026, 11, 4),
                CreatedAt = new DateTime(2026, 8, 30),
                IsPublic = false,
                NumberOfLocations = 9
            }
        };


        // ==============================
        // DANH SÁCH ĐỊA ĐIỂM TRONG LỊCH TRÌNH
        // ==============================

        private static List<ItineraryItem> itineraryItems = new List<ItineraryItem>
        {
            // ==========================
            // LỊCH TRÌNH 1 - ĐÀ NẴNG
            // ==========================

            new ItineraryItem
            {
                Id = 1,
                ItineraryId = 1,
                LocationId = 1,
                LocationName = "Bán đảo Sơn Trà",
                Day = "Ngày 1",
                Time = "08:00",
                Note = "Tham quan và ngắm cảnh biển từ bán đảo Sơn Trà.",
                OrderIndex = 1
            },

            new ItineraryItem
            {
                Id = 2,
                ItineraryId = 1,
                LocationId = 1,
                LocationName = "Biển Mỹ Khê",
                Day = "Ngày 1",
                Time = "14:00",
                Note = "Tắm biển và nghỉ ngơi.",
                OrderIndex = 2
            },

            new ItineraryItem
            {
                Id = 3,
                ItineraryId = 1,
                LocationId = 1,
                LocationName = "Cầu Rồng",
                Day = "Ngày 1",
                Time = "19:00",
                Note = "Tham quan Cầu Rồng và khu vực trung tâm thành phố.",
                OrderIndex = 3
            },

            new ItineraryItem
            {
                Id = 4,
                ItineraryId = 1,
                LocationId = 1,
                LocationName = "Bà Nà Hills",
                Day = "Ngày 2",
                Time = "08:00",
                Note = "Khám phá Bà Nà Hills và Cầu Vàng.",
                OrderIndex = 4
            },

            new ItineraryItem
            {
                Id = 5,
                ItineraryId = 1,
                LocationId = 1,
                LocationName = "Chợ đêm Sơn Trà",
                Day = "Ngày 2",
                Time = "19:00",
                Note = "Ăn uống và mua sắm tại chợ đêm.",
                OrderIndex = 5
            },

            new ItineraryItem
            {
                Id = 6,
                ItineraryId = 1,
                LocationId = 1,
                LocationName = "Ngũ Hành Sơn",
                Day = "Ngày 3",
                Time = "08:30",
                Note = "Tham quan danh thắng Ngũ Hành Sơn.",
                OrderIndex = 6
            },

            new ItineraryItem
            {
                Id = 7,
                ItineraryId = 1,
                LocationId = 6,
                LocationName = "Phố cổ Hội An",
                Day = "Ngày 3",
                Time = "15:00",
                Note = "Khám phá phố cổ Hội An và thưởng thức ẩm thực địa phương.",
                OrderIndex = 7
            },


            // ==========================
            // LỊCH TRÌNH 2 - ĐÀ LẠT
            // ==========================

            new ItineraryItem
            {
                Id = 8,
                ItineraryId = 2,
                LocationId = 2,
                LocationName = "Hồ Xuân Hương",
                Day = "Ngày 1",
                Time = "08:00",
                Note = "Đi dạo quanh hồ và ngắm cảnh.",
                OrderIndex = 1
            },

            new ItineraryItem
            {
                Id = 9,
                ItineraryId = 2,
                LocationId = 2,
                LocationName = "Quảng trường Lâm Viên",
                Day = "Ngày 1",
                Time = "14:00",
                Note = "Check-in và tham quan quảng trường.",
                OrderIndex = 2
            },

            new ItineraryItem
            {
                Id = 10,
                ItineraryId = 2,
                LocationId = 2,
                LocationName = "Chợ Đà Lạt",
                Day = "Ngày 1",
                Time = "19:00",
                Note = "Khám phá ẩm thực đường phố.",
                OrderIndex = 3
            },

            new ItineraryItem
            {
                Id = 11,
                ItineraryId = 2,
                LocationId = 2,
                LocationName = "Đồi chè Cầu Đất",
                Day = "Ngày 2",
                Time = "08:00",
                Note = "Ngắm cảnh và chụp ảnh.",
                OrderIndex = 4
            },

            new ItineraryItem
            {
                Id = 12,
                ItineraryId = 2,
                LocationId = 2,
                LocationName = "Thác Datanla",
                Day = "Ngày 2",
                Time = "14:00",
                Note = "Tham quan thác và trải nghiệm các hoạt động.",
                OrderIndex = 5
            }
        };


        // ==============================
        // INDEX
        // ==============================

        public IActionResult Index()
        {
            return View(itineraries);
        }


        // ==============================
        // DETAILS
        // ==============================

        public IActionResult Details(int id)
        {
            var itinerary = itineraries
                .FirstOrDefault(x => x.Id == id);

            if (itinerary == null)
            {
                return NotFound();
            }

            var items = itineraryItems
                .Where(x => x.ItineraryId == id)
                .OrderBy(x => x.OrderIndex)
                .ToList();

            ViewBag.Items = items;

            return View(itinerary);
        }


        // ==============================
        // CREATE - GET
        // ==============================

        public IActionResult Create()
        {
            return View();
        }


        // ==============================
        // CREATE - POST
        // ==============================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Itinerary itinerary)
        {
            if (itinerary.StartDate > itinerary.EndDate)
            {
                ModelState.AddModelError(
                    "EndDate",
                    "Ngày kết thúc phải sau hoặc bằng ngày bắt đầu."
                );
            }

            if (!ModelState.IsValid)
            {
                return View(itinerary);
            }

            itinerary.Id = itineraries.Count > 0
                ? itineraries.Max(x => x.Id) + 1
                : 1;

            itinerary.UserId = 1;
            itinerary.CreatedAt = DateTime.Now;
            itinerary.NumberOfLocations = 0;

            itineraries.Add(itinerary);

            return RedirectToAction(
                "Details",
                new { id = itinerary.Id }
            );
        }

        // ==============================
        // ADD LOCATION - GET
        // ==============================

        [HttpGet]
        public IActionResult AddLocation(int itineraryId)
        {
            var itinerary = itineraries
                .FirstOrDefault(x => x.Id == itineraryId);

            if (itinerary == null)
            {
                return NotFound();
            }

            ViewBag.Itinerary = itinerary;

            return View(new ItineraryItem
            {
                ItineraryId = itineraryId,
                Day = "Ngày 1",
                Time = "08:00"
            });
        }


        // ==============================
        // ADD LOCATION - POST
        // ==============================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddLocation(ItineraryItem item)
        {
            var itinerary = itineraries
                .FirstOrDefault(x => x.Id == item.ItineraryId);

            if (itinerary == null)
            {
                return NotFound();
            }

            // Danh sách địa điểm mẫu
            var locations = new Dictionary<int, string>
    {
        { 1, "Đà Nẵng" },
        { 2, "Đà Lạt" },
        { 3, "Vịnh Hạ Long" },
        { 4, "Sapa" },
        { 5, "Phú Quốc" },
        { 6, "Hội An" }
    };

            if (!locations.ContainsKey(item.LocationId))
            {
                ModelState.AddModelError(
                    "LocationId",
                    "Vui lòng chọn một địa điểm."
                );
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Itinerary = itinerary;
                return View(item);
            }

            item.Id = itineraryItems.Count > 0
                ? itineraryItems.Max(x => x.Id) + 1
                : 1;

            item.LocationName = locations[item.LocationId];

            item.OrderIndex = itineraryItems
                .Where(x => x.ItineraryId == item.ItineraryId)
                .Count() + 1;

            itineraryItems.Add(item);

            // Cập nhật số lượng địa điểm
            itinerary.NumberOfLocations = itineraryItems
                .Count(x => x.ItineraryId == itinerary.Id);

            return RedirectToAction(
                "Details",
                new { id = item.ItineraryId }
            );
        }

    }
}