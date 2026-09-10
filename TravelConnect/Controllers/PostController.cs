using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelConnect.Data;
using TravelConnect.Models;

namespace TravelConnect.Controllers
{
    public class PostController : Controller
    {
        private readonly TravelConnectDbContext _context;

        public PostController(
            TravelConnectDbContext context)
        {
            _context = context;
        }

        // =========================
        // DANH SÁCH BÀI VIẾT
        // =========================
        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(posts);
        }

        // =========================
        // CHI TIẾT BÀI VIẾT
        // =========================
        public async Task<IActionResult> Details(int id)
        {
            var post = await _context.Posts
                .FirstOrDefaultAsync(x => x.Id == id);

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
            int? userId =
                HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account"
                );
            }

            return View();
        }

        // =========================
        // TẠO BÀI VIẾT - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            string title,
            string content,
            string image,
            string locationName)
        {
            int? userId =
                HttpContext.Session.GetInt32("UserId");

            string? userName =
                HttpContext.Session.GetString("UserName");

            if (userId == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account"
                );
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                ViewBag.Error =
                    "Vui lòng nhập tiêu đề.";

                return View();
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                ViewBag.Error =
                    "Vui lòng nhập nội dung.";

                return View();
            }

            var post = new Post
            {
                UserId = userId.Value,

                UserName = userName ?? "Người dùng",

                Title = title.Trim(),

                Content = content.Trim(),

                Image = image?.Trim() ?? "",

                LocationName =
                    locationName?.Trim() ?? "",

                CreatedAt = DateTime.Now,

                LikeCount = 0,

                CommentCount = 0
            };

            _context.Posts.Add(post);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // =========================
        // LIKE / UNLIKE
        // =========================
        public async Task<IActionResult> Like(int id)
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

            var post =
                await _context.Posts
                .FirstOrDefaultAsync(x => x.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            var existingLike =
                await _context.PostLikes
                .FirstOrDefaultAsync(x =>
                    x.PostId == id &&
                    x.UserId == userId.Value
                );

            // Đã like → bỏ like
            if (existingLike != null)
            {
                _context.PostLikes.Remove(
                    existingLike
                );

                if (post.LikeCount > 0)
                {
                    post.LikeCount--;
                }
            }
            else
            {
                // Chưa like → thêm like
                var like = new PostLike
                {
                    PostId = id,
                    UserId = userId.Value
                };

                _context.PostLikes.Add(like);

                post.LikeCount++;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                new { id = id }
            );
        }

        // =========================
        // KIỂM TRA USER ĐÃ LIKE
        // =========================
        public async Task<bool> IsLiked(
            int postId,
            int userId)
        {
            return await _context.PostLikes
                .AnyAsync(x =>
                    x.PostId == postId &&
                    x.UserId == userId
                );
        }

        // =========================
        // HÀM TƯƠNG THÍCH CHO CODE CŨ
        // =========================
        public static List<Post> GetPostList()
        {
            return new List<Post>();
        }

        // =========================
        // HÀM TƯƠNG THÍCH CHO DETAILS.CSHTML
        // =========================
        public static bool IsPostLiked(
            int postId,
            int userId)
        {
            return false;
        }
    }
}