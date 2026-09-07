namespace TravelConnect.Models
{
    public class Post
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; } = "";

        public string Title { get; set; } = "";

        public string Content { get; set; } = "";

        public string Image { get; set; } = "";

        public string LocationName { get; set; } = "";

        public DateTime CreatedAt { get; set; }

        public int LikeCount { get; set; }

        public int CommentCount { get; set; }
    }
}