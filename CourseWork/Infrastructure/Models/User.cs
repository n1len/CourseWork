using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CourseWork.Infrastructure.Models
{
    // Add profile data for application users by adding properties to the User class
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<CustomCollection> CustomCollections { get; set; }
    }
}
