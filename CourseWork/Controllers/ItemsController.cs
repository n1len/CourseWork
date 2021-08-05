using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseWork.Data;
using CourseWork.Infrastructure.Models;
using CourseWork.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CourseWork.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public ItemsController(ApplicationContext context,UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Item
                .Include(i => i.CustomCollection);
            return View(await applicationContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var itemViewModel = await GetItemViewModel(id);

            if (itemViewModel.Item == null)
                return NotFound();

            return View(itemViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Details(int id,ItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var userId = user.Id;
                DbObjects.CreateComment(_context, model.Description, id, userId);
            }

            return RedirectToAction("Details");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int id, Item item)
        {
            if (ModelState.IsValid)
            {
                DbObjects.CreateItem(_context,item,id);
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var item = await _context.Item.FindAsync(id);
            if (item == null)
                return NotFound();

            ViewData["CustomCollectionId"] = new SelectList(_context.CustomCollection, "Id", "Id", item.CustomCollectionId);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Tags,NumericField1,NumericField2,NumericField3,OneLineField1,OneLineField2,OneLineField3,TextField1,TextField2,TextField3,Date1,Date2,Date3,CheckBox1,CheckBox2,CheckBox3,CustomCollectionId")] Item item)
        {
            if (id != item.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomCollectionId"] = new SelectList(_context.CustomCollection, "Id", "Id", item.CustomCollectionId);
            return View(item);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var item = await _context.Item
                .Include(i => i.CustomCollection)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (item == null)
                return NotFound();

            return View(item);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Item.FindAsync(id);
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Id == id);
        }

        private async Task<ItemViewModel> GetItemViewModel(int? id)
        {
            var item = await _context.Item
                .Include(i => i.CustomCollection)
                .FirstOrDefaultAsync(m => m.Id == id);

            var comments = _context.Comment
                .Include(u => u.User)
                .Include(i => i.Item)
                .Where(m => m.ItemId == id);

            var itemViewModel = new ItemViewModel
            {
                Item = item,
                Comments = comments
            };

            return itemViewModel;
        }
    }
}
