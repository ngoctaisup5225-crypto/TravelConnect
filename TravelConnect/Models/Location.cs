namespace TravelConnect.Models
{
    public class Location
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Province { get; set; } = "";

        public string Category { get; set; } = "";

        public string Description { get; set; } = "";

        public string Image { get; set; } = "";

        public double Rating { get; set; }
    }
}