namespace CourseWork.Infrastructure.Models
{
    public class Like
    {
        public int Id { get; set; }
        public uint Amount { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
