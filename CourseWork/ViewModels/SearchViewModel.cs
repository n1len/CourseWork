using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
