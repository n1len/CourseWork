using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.Infrastructure.Models;

namespace CourseWork.ViewModels
{
    public class ItemViewModel
    {
        public Item Item { get; set; }
        public IEnumerable<Item> Items { get; set; }

        [Required(ErrorMessage = "Комментарий не может быть пустым")]
        public string Description { get; set; }

        public CustomCollection CustomCollection { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<LikeOnItem> LikesOnItem { get; set; }
    }
}
