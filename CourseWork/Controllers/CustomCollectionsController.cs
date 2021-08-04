using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseWork.Data;
using CourseWork.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace CourseWork.Controllers
{
    public class CustomCollectionsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public CustomCollectionsController(ApplicationContext context,UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.CustomCollection.Include(c => c.User);
            return View(await applicationContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customCollection = await _context.CustomCollection
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customCollection == null)
            {
                return NotFound();
            }

            return View(customCollection);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Topic,Img,IsNumericField1Visible,IsNumericField2Visible,IsNumericField3Visible,IsOneLineField1Visible,IsOneLineField2Visible,IsOneLineField3Visible,IsTextField1Visible,IsTextField2Visible,IsTextField3Visible,IsDate1Visible,IsDate2Visible,IsDate3Visible,IsCheckBox1Visible,IsCheckBox2Visible,IsCheckBox3Visible")] CustomCollection customCollection)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                customCollection.UserId = user.Id;
                _context.Add(customCollection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customCollection);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customCollection = await _context.CustomCollection.FindAsync(id);
            if (customCollection == null)
            {
                return NotFound();
            }
            return View(customCollection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Topic,Img,IsNumericField1Visible,IsNumericField2Visible,IsNumericField3Visible,IsOneLineField1Visible,IsOneLineField2Visible,IsOneLineField3Visible,IsTextField1Visible,IsTextField2Visible,IsTextField3Visible,IsDate1Visible,IsDate2Visible,IsDate3Visible,IsCheckBox1Visible,IsCheckBox2Visible,IsCheckBox3Visible")] CustomCollection customCollection)
        {
            if (id != customCollection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(User.Identity.Name);
                    customCollection.UserId = user.Id;
                    _context.Update(customCollection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomCollectionExists(customCollection.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customCollection);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customCollection = await _context.CustomCollection
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customCollection == null)
            {
                return NotFound();
            }

            return View(customCollection);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customCollection = await _context.CustomCollection.FindAsync(id);
            _context.CustomCollection.Remove(customCollection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomCollectionExists(int id)
        {
            return _context.CustomCollection.Any(e => e.Id == id);
        }
    }
}
