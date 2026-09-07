namespace TravelConnect.Models
{
    public class Itinerary
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsPublic { get; set; }

        public int NumberOfLocations { get; set; }
    }
}