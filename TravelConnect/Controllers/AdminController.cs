using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelConnect.Data;
using TravelConnect.Models;

namespace TravelConnect.Controllers
{
    public class AdminController : Controller
    {
        private readonly TravelConnectDbContext _context;

        public AdminController(TravelConnectDbContext context)
        {
            _context = context;
        }

        // =========================
        // KIỂM TRA ADMIN
        // =========================
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        private IActionResult? CheckAdmin()
        {
            int? userId =
                HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction(
                    "Login",
                    "Account");
            }

            if (!IsAdmin())
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }

            return null;
        }


        // =========================
        // DASHBOARD
        // =========================
        public async Task<IActionResult> Index()
        {
            var check = CheckAdmin();

            if (check != null)
                return check;

            ViewBag.UserCount =
                await _context.Users.CountAsync();

            ViewBag.LocationCount =
                await _context.Locations.CountAsync();

            ViewBag.PostCount =
                await _context.Posts.CountAsync();

            ViewBag.CommentCount =
                await _context.Comments.CountAsync();

            ViewBag.FavoriteCount =
                await _context.Favorites.CountAsync();

            return View();
        }


        // =========================
        // USERS
        // =========================
        public async Task<IActionResult> Users()
        {
            var check = CheckAdmin();

            if (check != null)
                return check;

            var users =
                await _context.Users
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return View(users);
        }


        // =========================
        // XÓA USER
        // =========================
        [HttpGet]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var check = CheckAdmin();

            if (check != null)
                return check;

            var user =
                await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user != null &&
                user.Role != "Admin")
            {
                _context.Users.Remove(user);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Users");
        }


        // =========================
        // POSTS
        // =========================
        public async Task<IActionResult> Posts()
        {
            var check = CheckAdmin();

            if (check != null)
                return check;

            var posts =
                await _context.Posts
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(posts);
        }


        // =========================
        // XÓA POST
        // =========================
        [HttpGet]
        public async Task<IActionResult> DeletePost(int id)
        {
            var check = CheckAdmin();

            if (check != null)
                return check;

            var post =
                await _context.Posts
                .FirstOrDefaultAsync(x => x.Id == id);

            if (post != null)
            {
                // Xóa các like của bài viết
                var likes =
                    await _context.PostLikes
                    .Where(x => x.PostId == id)
                    .ToListAsync();

                if (likes.Any())
                {
                    _context.PostLikes.RemoveRange(likes);
                }


                // Xóa các comment của bài viết
                var comments =
                    await _context.Comments
                    .Where(x => x.PostId == id)
                    .ToListAsync();

                if (comments.Any())
                {
                    _context.Comments.RemoveRange(comments);
                }


                // Xóa bài viết
                _context.Posts.Remove(post);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Posts");
        }


        // =========================
        // COMMENTS
        // =========================
        public async Task<IActionResult> Comments()
        {
            var check = CheckAdmin();

            if (check != null)
                return check;

            var comments =
                await _context.Comments
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(comments);
        }


        // =========================
        // XÓA COMMENT
        // =========================
        [HttpGet]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var check = CheckAdmin();

            if (check != null)
                return check;

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

            return RedirectToAction("Comments");
        }
    }
}