namespace TravelConnect.Models
{
    public class ItineraryItem
    {
        public int Id { get; set; }

        public int ItineraryId { get; set; }

        public int LocationId { get; set; }

        public string LocationName { get; set; } = "";

        public string Day { get; set; } = "";

        public string Time { get; set; } = "";

        public string Note { get; set; } = "";

        public int OrderIndex { get; set; }
    }
}