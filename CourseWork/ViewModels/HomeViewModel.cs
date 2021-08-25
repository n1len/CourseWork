using System.Collections.Generic;
using CourseWork.Infrastructure.Models;

namespace CourseWork.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<CustomCollection> CustomCollections { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
