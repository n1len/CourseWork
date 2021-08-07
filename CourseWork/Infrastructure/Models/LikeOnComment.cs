namespace CourseWork.Infrastructure.Models
{
    public class LikeOnComment
    {
        public int Id { get; set; }
        public bool IsLiked { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
