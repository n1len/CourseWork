using System.Collections.Generic;

namespace CourseWork.Infrastructure.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<LikeOnComment> Likes { get; set; }
    }
}
