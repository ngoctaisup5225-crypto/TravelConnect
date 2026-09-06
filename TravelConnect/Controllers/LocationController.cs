using Microsoft.AspNetCore.Mvc;
using TravelConnect.Models;

namespace TravelConnect.Controllers
{
    public class LocationController : Controller
    {
        // Danh sách địa điểm mẫu
        private List<Location> GetLocations()
        {
            return new List<Location>
            {
                new Location
                {
                    Id = 1,
                    Name = "Đà Nẵng",
                    Province = "Đà Nẵng",
                    Category = "Biển",
                    Description = "Thành phố biển xinh đẹp với những bãi biển nổi tiếng, cầu Rồng và nhiều điểm tham quan hấp dẫn.",
                    Image = "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?auto=format&fit=crop&w=1000&q=80",
                    Rating = 4.8
                },

                new Location
                {
                    Id = 2,
                    Name = "Đà Lạt",
                    Province = "Lâm Đồng",
                    Category = "Thiên nhiên",
                    Description = "Thành phố ngàn hoa với khí hậu mát mẻ, cảnh quan lãng mạn và nhiều địa điểm check-in nổi tiếng.",
                    Image = "https://images.unsplash.com/photo-1599707254554-027aeb4deacd?auto=format&fit=crop&w=1000&q=80",
                    Rating = 4.7
                },

                new Location
                {
                    Id = 3,
                    Name = "Vịnh Hạ Long",
                    Province = "Quảng Ninh",
                    Category = "Biển",
                    Description = "Di sản thiên nhiên nổi tiếng với hàng nghìn đảo đá vôi và cảnh quan tuyệt đẹp trên biển.",
                    Image = "https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=1000&q=80",
                    Rating = 4.9
                },

                new Location
                {
                    Id = 4,
                    Name = "Sapa",
                    Province = "Lào Cai",
                    Category = "Núi",
                    Description = "Vùng núi phía Bắc nổi tiếng với ruộng bậc thang, khí hậu mát mẻ và văn hóa đặc sắc của các dân tộc.",
                    Image = "https://images.unsplash.com/photo-1573270689103-d7a4e42b609a?auto=format&fit=crop&w=1000&q=80",
                    Rating = 4.6
                },

                new Location
                {
                    Id = 5,
                    Name = "Phú Quốc",
                    Province = "Kiên Giang",
                    Category = "Biển",
                    Description = "Đảo ngọc nổi tiếng với biển xanh, cát trắng, hoàng hôn đẹp và nhiều hoạt động vui chơi.",
                    Image = "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?auto=format&fit=crop&w=1000&q=80",
                    Rating = 4.8
                },

                new Location
                {
                    Id = 6,
                    Name = "Hội An",
                    Province = "Quảng Nam",
                    Category = "Văn hóa",
                    Description = "Phố cổ mang vẻ đẹp truyền thống với những ngôi nhà cổ, đèn lồng và nền văn hóa đặc sắc.",
                    Image = "https://images.unsplash.com/photo-1528181304800-259b08848526?auto=format&fit=crop&w=1000&q=80",
                    Rating = 4.9
                }
            };
        }

        // GET: /Location
        // GET: /Location/Index
        public IActionResult Index(string search, string category)
        {
            var locations = GetLocations();

            // Tìm kiếm
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                locations = locations
                    .Where(x =>
                        x.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        x.Province.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Lọc theo danh mục
            if (!string.IsNullOrWhiteSpace(category) &&
                category != "Tất cả")
            {
                locations = locations
                    .Where(x =>
                        x.Category.Equals(
                            category,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Gửi dữ liệu tìm kiếm/lọc về View
            ViewBag.Search = search;
            ViewBag.Category = category;

            return View(locations);
        }

        // GET: /Location/Details/1
        public IActionResult Details(int id)
        {
            var locations = GetLocations();

            var location = locations.FirstOrDefault(x => x.Id == id);

            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }
    }
}