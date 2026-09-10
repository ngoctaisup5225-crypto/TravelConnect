using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelConnect.Data;
using TravelConnect.Models;

namespace TravelConnect.Controllers
{
    public class CommentController : Controller
    {
        private readonly TravelConnectDbContext _context;

        public CommentController(TravelConnectDbContext context)
        {
            _context = context;
        }

        // =========================
        // THÊM BÌNH LUẬN
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            int postId,
            string content)
        {
            int? userId =
                HttpContext.Session.GetInt32("UserId");

            string? userName =
                HttpContext.Session.GetString("UserName");

            // Chưa đăng nhập
            if (userId == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account");
            }

            // Nội dung rỗng
            if (string.IsNullOrWhiteSpace(content))
            {
                return RedirectToAction(
                    "Details",
                    "Post",
                    new { id = postId });
            }

            // Kiểm tra bài viết
            var post =
                await _context.Posts
                .FirstOrDefaultAsync(x => x.Id == postId);

            if (post == null)
            {
                return NotFound();
            }

            // Tạo comment
            var comment = new Comment
            {
                PostId = postId,
                UserId = userId.Value,
                UserName = userName ?? "Người dùng",
                Content = content.Trim(),
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);

            // Cập nhật số comment
            post.CommentCount++;

            await _context.SaveChangesAsync();

            return RedirectToAction(
                "Details",
                "Post",
                new { id = postId });
        }


        // =========================
        // XÓA BÌNH LUẬN CỦA USER
        // =========================
        [HttpGet]
        public async Task<IActionResult> Remove(
            int id,
            int postId)
        {
            int? userId =
                HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account");
            }

            var comment =
                await _context.Comments
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.UserId == userId.Value);

            if (comment != null)
            {
                var post =
                    await _context.Posts
                    .FirstOrDefaultAsync(x =>
                        x.Id == comment.PostId);

                _context.Comments.Remove(comment);

                if (post != null &&
                    post.CommentCount > 0)
                {
                    post.CommentCount--;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(
                "Details",
                "Post",
                new { id = postId });
        }


        // =========================
        // LẤY COMMENT THEO POST
        // =========================
        public async Task<List<Comment>> GetComments(
            int postId)
        {
            return await _context.Comments
                .Where(x => x.PostId == postId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }


        // =========================
        // LẤY TẤT CẢ COMMENT
        // DÙNG CHO ADMIN
        // =========================
        public async Task<List<Comment>> GetAllComments()
        {
            return await _context.Comments
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }


        // =========================
        // ADMIN XÓA COMMENT
        // =========================
        [HttpGet]
        public async Task<IActionResult> AdminRemove(
            int id)
        {
            string? role =
                HttpContext.Session.GetString("UserRole");

            if (role != "Admin")
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            var comment =
                await _context.Comments
                .FirstOrDefaultAsync(x => x.Id == id);

            if (comment != null)
            {
                var post =
                    await _context.Posts
                    .FirstOrDefaultAsync(x =>
                        x.Id == comment.PostId);

                _context.Comments.Remove(comment);

                if (post != null &&
                    post.CommentCount > 0)
                {
                    post.CommentCount--;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(
                "Comments",
                "Admin");
        }
    }
}