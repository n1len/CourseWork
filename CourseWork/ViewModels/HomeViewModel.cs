using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
