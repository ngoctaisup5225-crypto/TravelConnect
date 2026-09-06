namespace TravelConnect.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Content { get; set; } = "";

        public string Author { get; set; } = "";

        public string Location { get; set; } = "";

        public string Image { get; set; } = "";

        public DateTime CreatedAt { get; set; }

        public int Likes { get; set; }

        public int Comments { get; set; }
    }
}