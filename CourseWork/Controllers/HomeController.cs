using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.Data;
using CourseWork.Infrastructure.Models;
using CourseWork.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;

        public HomeController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var collections = GetTopCollections();
            var items = GetLastAddedItems();
            var tags = GetUniqueTags();

            var homeViewModel = new HomeViewModel
            {
                CustomCollections = collections,
                Items = items,
                Tags = tags
            };
            return View(homeViewModel);
        }

        public IActionResult Search(string query)
        {
            var items = _context.Item
                .Include(c => c.CustomCollection)
                .Where(x => x.Title.Contains(query) || x.Tags.Contains(query) || x.NumericField1.Contains(query)
                            || x.NumericField2.Contains(query) || x.NumericField3.Contains(query) || x.OneLineField1.Contains(query)
                            || x.OneLineField2.Contains(query) || x.OneLineField3.Contains(query) || x.TextField1.Contains(query)
                            || x.TextField2.Contains(query) || x.TextField3.Contains(query) || x.Date1.Contains(query)
                            || x.Date2.Contains(query) || x.Date3.Contains(query) || x.CustomCollection.Description.Contains(query));
            var comments = _context.Comment
                .Include(i => i.Item)
                .Where(c => c.Description.Contains(query));

            var search = new SearchViewModel
            {
                Items = items,
                Comments = comments,
                Query = query
            };

            return View(search);
        }

        private IEnumerable<CustomCollection> GetTopCollections()
        {
            var collections = _context.CustomCollection
                .Include(i => i.Items)
                .OrderByDescending(i => i.Items.Count).Take(3);

            return collections;
        }

        private IEnumerable<Item> GetLastAddedItems()
        {
            var items = _context.Item
                .OrderByDescending(i => i.Id).Take(4);

            return items;
        }

        private IEnumerable<string> GetUniqueTags()
        {
            var tags = _context.Item
                .Select(i => i.Tags)
                .Distinct().Take(10);

            return tags;
        }
    }
}
