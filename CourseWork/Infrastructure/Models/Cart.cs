using System;
using System.Collections.Generic;

namespace CourseWork.Infrastructure.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<Item> ItemsInCart { get; set; }
    }
}
