namespace CourseWork.Infrastructure.Models
{
    public class LikeOnItem
    {
        public int Id { get; set; }
        public bool IsLiked { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
