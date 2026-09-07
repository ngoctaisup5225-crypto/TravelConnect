using Microsoft.AspNetCore.Mvc;
using TravelConnect.Models;

namespace TravelConnect.Controllers
{
    public class PostController : Controller
    {
        private static List<Post> posts = new List<Post>
        {
            new Post
            {
                Id = 1,
                UserId = 1,
                UserName = "Nguyễn Minh",
                Title = "Một ngày chill ở Đà Lạt 🌲",
                Content = "Đà Lạt thật sự rất đẹp. Không khí mát mẻ, đồ ăn ngon và có rất nhiều địa điểm để khám phá.",
                Image = "https://images.unsplash.com/photo-1552521001-4d7e4b1d8c8a?auto=format&fit=crop&w=1000&q=80",
                LocationName = "Đà Lạt",
                CreatedAt = DateTime.Now.AddDays(-2),
                LikeCount = 15,
                CommentCount = 4
            },

            new Post
            {
                Id = 2,
                UserId = 2,
                UserName = "Hoàng Anh",
                Title = "Kinh nghiệm du lịch Phú Quốc 🏝️",
                Content = "Nếu có thời gian thì mọi người nên dành ít nhất 3 ngày để khám phá Phú Quốc. Biển rất đẹp và đồ ăn khá ngon.",
                Image = "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?auto=format&fit=crop&w=1000&q=80",
                LocationName = "Phú Quốc",
                CreatedAt = DateTime.Now.AddDays(-5),
                LikeCount = 23,
                CommentCount = 7
            },

            new Post
            {
                Id = 3,
                UserId = 3,
                UserName = "Tuấn Travel",
                Title = "Sapa mùa này có gì? ⛰️",
                Content = "Sapa thời tiết khá lạnh nhưng cảnh núi rất đẹp. Phù hợp với những ai thích khám phá và trekking.",
                Image = "https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=1000&q=80",
                LocationName = "Sapa",
                CreatedAt = DateTime.Now.AddDays(-8),
                LikeCount = 31,
                CommentCount = 9
            }
        };


        // =========================
        // DANH SÁCH BÀI VIẾT
        // =========================

        public IActionResult Index()
        {
            var result = posts
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            return View(result);
        }


        // =========================
        // CHI TIẾT BÀI VIẾT
        // =========================

        public IActionResult Details(int id)
        {
            var post = posts
                .FirstOrDefault(x => x.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }


        // =========================
        // TẠO BÀI VIẾT - GET
        // =========================

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // =========================
        // TẠO BÀI VIẾT - POST
        // =========================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Post post)
        {
            if (string.IsNullOrWhiteSpace(post.Title))
            {
                ModelState.AddModelError(
                    "Title",
                    "Vui lòng nhập tiêu đề bài viết."
                );
            }

            if (string.IsNullOrWhiteSpace(post.Content))
            {
                ModelState.AddModelError(
                    "Content",
                    "Vui lòng nhập nội dung bài viết."
                );
            }

            if (!ModelState.IsValid)
            {
                return View(post);
            }

            post.Id = posts.Count > 0
                ? posts.Max(x => x.Id) + 1
                : 1;

            // Tạm thời sử dụng UserId = 1
            // Sau này làm đăng nhập sẽ lấy UserId từ tài khoản đang đăng nhập
            post.UserId = 1;

            post.UserName = "Người dùng";

            post.CreatedAt = DateTime.Now;

            post.LikeCount = 0;

            post.CommentCount = 0;

            posts.Add(post);

            return RedirectToAction("Details", new
            {
                id = post.Id
            });
        }
    }
}