using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseWork.Data;
using CourseWork.Infrastructure.Models;
using CourseWork.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace CourseWork.Controllers
{
    public class CustomCollectionsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private static Cloudinary cloudinary;
        private static string cloud_name = "dpmauufdt";
        private static string api_key = "378443462439775";
        private static string api_secret = "2-bzZf2fR5Mmm9mCcgRFOnuSaps";

        public CustomCollectionsController(ApplicationContext context, UserManager<User> userManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Topic,Img,IsNumericField1Visible,IsNumericField2Visible,IsNumericField3Visible,IsOneLineField1Visible,IsOneLineField2Visible,IsOneLineField3Visible,IsTextField1Visible,IsTextField2Visible,IsTextField3Visible,IsDate1Visible,IsDate2Visible,IsDate3Visible,IsCheckBox1Visible,IsCheckBox2Visible,IsCheckBox3Visible,NumericField1,NumericField2,NumericField3,OneLineField1,OneLineField2,OneLineField3,TextField1,TextField2,TextField3,Date1,Date2,Date3,CheckBox1,CheckBox2,CheckBox3")] CustomCollection customCollection,string userName,CollectionViewModel viewModel)
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
                var fileName = await UploadedImage(viewModel);
                customCollection.Img = fileName;

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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Img,Topic,IsNumericField1Visible,IsNumericField2Visible,IsNumericField3Visible,IsOneLineField1Visible,IsOneLineField2Visible,IsOneLineField3Visible,IsTextField1Visible,IsTextField2Visible,IsTextField3Visible,IsDate1Visible,IsDate2Visible,IsDate3Visible,IsCheckBox1Visible,IsCheckBox2Visible,IsCheckBox3Visible,NumericField1,NumericField2,NumericField3,OneLineField1,OneLineField2,OneLineField3,TextField1,TextField2,TextField3,Date1,Date2,Date3,CheckBox1,CheckBox2,CheckBox3")] CustomCollection customCollection, string userName, string imageUrl, CollectionViewModel viewModel)
        {
            if (id != customCollection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var fileName = await UploadedImage(viewModel);
                    customCollection.Img = fileName ?? imageUrl;
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

        private async Task<string> UploadedImage(CollectionViewModel model)
        {
            string fileName = null;
            string filePath = null;
            var fileDir = "images";
            if (model.CollectionImage != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, fileDir);
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(fileDir);
                fileName = Guid.NewGuid().ToString() + "_" + model.CollectionImage.FileName;
                filePath = Path.Combine(uploadsFolder, fileName);
                await using var fileStream = new FileStream(filePath, FileMode.Create);
                await model.CollectionImage.CopyToAsync(fileStream);
            }

            if (filePath != null)
            {
                Account account = new Account(cloud_name, api_key, api_secret);
                cloudinary = new Cloudinary(account);
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(filePath),
                    PublicId = fileName
                };

                await cloudinary.UploadAsync(uploadParams);

                string url = cloudinary.Api.UrlImgUp.BuildUrl(fileName + ".jpg");
                fileName = url;
            }

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            return fileName;
        }
    }
}
