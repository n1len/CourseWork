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

        public IEnumerable<CustomCollection> GetTopCollections()
        {
            var collections = _context.CustomCollection
                .Include(i => i.Items)
                .OrderByDescending(i => i.Items.Count).Take(3);

            return collections;
        }

        public IEnumerable<Item> GetLastAddedItems()
        {
            var items = _context.Item
                .OrderByDescending(i => i.Id).Take(4);

            return items;
        }

        public IEnumerable<string> GetUniqueTags()
        {
            var tags = _context.Item
                .Select(i => i.Tags)
                .Distinct().Take(10);

            return tags;
        }
    }
}
