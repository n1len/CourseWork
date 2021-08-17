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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CourseWork.Controllers
{
    public class CustomCollectionsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public CustomCollectionsController(ApplicationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customCollection = await _context.CustomCollection
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!User.IsInRole("admin"))
            {
                if (customCollection == null || customCollection.UserId != user.Id)
                    return NotFound();
            }
            if (customCollection == null)
                return NotFound();


            return View(customCollection);
        }

        public async Task<IActionResult> Show(int? id)
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

        [Authorize]
        public IActionResult Create(string userName)
        {
            if (userName == null)
                return NotFound();
            var model = new CollectionViewModel
            {
                UserName = userName
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Topic,Img,IsNumericField1Visible,IsNumericField2Visible,IsNumericField3Visible,IsOneLineField1Visible,IsOneLineField2Visible,IsOneLineField3Visible,IsTextField1Visible,IsTextField2Visible,IsTextField3Visible,IsDate1Visible,IsDate2Visible,IsDate3Visible,IsCheckBox1Visible,IsCheckBox2Visible,IsCheckBox3Visible,NumericField1,NumericField2,NumericField3,OneLineField1,OneLineField2,OneLineField3,TextField1,TextField2,TextField3,Date1,Date2,Date3,CheckBox1,CheckBox2,CheckBox3")] CustomCollection customCollection,string userName)
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("admin"))
                {
                    var user = await _userManager.FindByNameAsync(userName);
                    if (user == null)
                        return NotFound();
                    customCollection.UserId = user.Id;
                }
                else
                {
                    var user = await _userManager.FindByNameAsync(User.Identity.Name);
                    if (user == null)
                        return NotFound();
                    customCollection.UserId = user.Id;
                }
                _context.Add(customCollection);
                await _context.SaveChangesAsync();
                return RedirectToAction("Personal","Account", new {UserName = userName});
            }

            var model = new CollectionViewModel
            {
                CustomCollection = customCollection,
                UserName = userName
            };
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id,string userName)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(userName);

            var customCollection = await _context.CustomCollection
                .Include(u => u.User)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (!User.IsInRole("admin"))
            {
                var tempUser = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user == null || user.UserName != tempUser.UserName)
                    return NotFound();

                if (customCollection == null || customCollection.UserId != user.Id)
                    return NotFound();
            }

            if (customCollection == null)
                return NotFound();

            var model = new CollectionViewModel
            {
                CustomCollection = customCollection,
                UserName = userName
            };

            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Topic,Img,IsNumericField1Visible,IsNumericField2Visible,IsNumericField3Visible,IsOneLineField1Visible,IsOneLineField2Visible,IsOneLineField3Visible,IsTextField1Visible,IsTextField2Visible,IsTextField3Visible,IsDate1Visible,IsDate2Visible,IsDate3Visible,IsCheckBox1Visible,IsCheckBox2Visible,IsCheckBox3Visible,NumericField1,NumericField2,NumericField3,OneLineField1,OneLineField2,OneLineField3,TextField1,TextField2,TextField3,Date1,Date2,Date3,CheckBox1,CheckBox2,CheckBox3")] CustomCollection customCollection, string userName)
        {
            if (id != customCollection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(userName);
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
                return RedirectToAction("Details",new {id = id});
            }

            var model = new CollectionViewModel
            {
                CustomCollection = customCollection,
                UserName = userName
            };
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customCollection = await _context.CustomCollection
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!User.IsInRole("admin"))
            {
                if (customCollection == null || customCollection.UserId != user.Id)
                    return NotFound();
            }
            if (customCollection == null)
                return NotFound();

            return View(customCollection);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customCollection = await _context.CustomCollection
                .Include(c => c.User)
                .FirstOrDefaultAsync(i => i.Id == id);
            var userName = customCollection.User.UserName;
            _context.CustomCollection.Remove(customCollection);
            await _context.SaveChangesAsync();
            return RedirectToAction("Personal","Account",new {UserName = userName });
        }

        private bool CustomCollectionExists(int id)
        {
            return _context.CustomCollection.Any(e => e.Id == id);
        }
    }
}
