using System.Collections.Generic;
using CourseWork.Infrastructure.Models;

namespace CourseWork.ViewModels
{
    public class SearchViewModel
    {
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public string Query { get; set; }
    }
}
