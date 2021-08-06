using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CourseWork.Infrastructure.Models
{
    // Add profile data for application users by adding properties to the User class
    public class User : IdentityUser
    {
        public bool IsBlocked { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<LikeOnComment> LikeOnComments { get; set; }
        public ICollection<LikeOnItem> LikeOnItems{ get; set; }
        public ICollection<CustomCollection> CustomCollections { get; set; }
    }
}
