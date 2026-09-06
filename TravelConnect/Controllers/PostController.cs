using Microsoft.AspNetCore.Mvc;
using TravelConnect.Models;

namespace TravelConnect.Controllers
{
    public class PostController : Controller
    {
        // Danh sách bài viết mẫu
        private List<Post> GetPosts()
        {
            return new List<Post>
            {
                new Post
                {
                    Id = 1,
                    Title = "Một ngày khám phá Đà Lạt",
                    Content = "Đà Lạt luôn là một trong những địa điểm mình yêu thích nhất. Không khí mát mẻ, cảnh đẹp và rất nhiều quán cà phê thú vị.",
                    Author = "Nguyễn Minh",
                    Location = "Đà Lạt",
                    Image = "https://images.unsplash.com/photo-1599707254554-027aeb4deacd?auto=format&fit=crop&w=1000&q=80",
                    CreatedAt = new DateTime(2026, 8, 20),
                    Likes = 125,
                    Comments = 18
                },

                new Post
                {
                    Id = 2,
                    Title = "Kinh nghiệm du lịch Phú Quốc 3 ngày 2 đêm",
                    Content = "Nếu bạn đang có kế hoạch đi Phú Quốc thì đây là lịch trình mình đã trải nghiệm. Mình sẽ chia sẻ những địa điểm ăn uống, vui chơi và nghỉ ngơi.",
                    Author = "Trần Hoàng",
                    Location = "Phú Quốc",
                    Image = "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?auto=format&fit=crop&w=1000&q=80",
                    CreatedAt = new DateTime(2026, 8, 15),
                    Likes = 98,
                    Comments = 12
                },

                new Post
                {
                    Id = 3,
                    Title = "Sapa mùa lúa chín có gì đẹp?",
                    Content = "Sapa vào mùa lúa chín thực sự rất đẹp. Những thửa ruộng bậc thang trải dài trên các sườn núi tạo nên khung cảnh cực kỳ ấn tượng.",
                    Author = "Lê Anh",
                    Location = "Sapa",
                    Image = "https://images.unsplash.com/photo-1573270689103-d7a4e42b609a?auto=format&fit=crop&w=1000&q=80",
                    CreatedAt = new DateTime(2026, 8, 10),
                    Likes = 156,
                    Comments = 25
                },

                new Post
                {
                    Id = 4,
                    Title = "Những địa điểm không thể bỏ qua ở Đà Nẵng",
                    Content = "Đà Nẵng có rất nhiều địa điểm đẹp như biển Mỹ Khê, cầu Rồng, bán đảo Sơn Trà và Bà Nà Hills.",
                    Author = "Phạm Tuấn",
                    Location = "Đà Nẵng",
                    Image = "https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?auto=format&fit=crop&w=1000&q=80",
                    CreatedAt = new DateTime(2026, 8, 5),
                    Likes = 87,
                    Comments = 9
                },

                new Post
                {
                    Id = 5,
                    Title = "Check-in phố cổ Hội An buổi tối",
                    Content = "Buổi tối ở Hội An rất đẹp với những con phố được thắp sáng bằng đèn lồng. Đây là địa điểm rất phù hợp để đi dạo và chụp ảnh.",
                    Author = "Ngọc Mai",
                    Location = "Hội An",
                    Image = "https://images.unsplash.com/photo-1528181304800-259b08848526?auto=format&fit=crop&w=1000&q=80",
                    CreatedAt = new DateTime(2026, 7, 28),
                    Likes = 112,
                    Comments = 15
                }
            };
        }

        // GET: /Post
        // GET: /Post/Index
        public IActionResult Index(string search)
        {
            var posts = GetPosts();

            // Tìm kiếm bài viết
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                posts = posts
                    .Where(x =>
                        x.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        x.Content.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        x.Location.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        x.Author.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewBag.Search = search;

            return View(posts);
        }

        // GET: /Post/Details/1
        public IActionResult Details(int id)
        {
            var posts = GetPosts();

            var post = posts.FirstOrDefault(x => x.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
    }
}