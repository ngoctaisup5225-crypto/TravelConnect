namespace TravelConnect.Models
{
    public class Favorite
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int LocationId { get; set; }

        public string LocationName { get; set; } = "";

        public string Province { get; set; } = "";

        public string Image { get; set; } = "";

        public double Rating { get; set; }
    }
}