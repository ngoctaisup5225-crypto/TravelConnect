using Microsoft.AspNetCore.Mvc;
using TravelConnect.Models;

namespace TravelConnect.Controllers
{
    public class FavoriteController : Controller
    {
        private static List<Favorite> favorites = new List<Favorite>();

        public IActionResult Index()
        {
            return View(favorites);
        }

        public IActionResult Add(
            int id,
            string name,
            string province,
            string image,
            double rating)
        {
            bool exists = favorites.Any(x =>
                x.LocationId == id &&
                x.UserId == 1);

            if (!exists)
            {
                favorites.Add(new Favorite
                {
                    Id = favorites.Count > 0
                        ? favorites.Max(x => x.Id) + 1
                        : 1,

                    UserId = 1,
                    LocationId = id,
                    LocationName = name,
                    Province = province,
                    Image = image,
                    Rating = rating
                });
            }

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var favorite = favorites.FirstOrDefault(x => x.Id == id);

            if (favorite != null)
            {
                favorites.Remove(favorite);
            }

            return RedirectToAction("Index");
        }
    }
}