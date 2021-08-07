﻿using System;
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
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        [Authorize]
        public async Task<IActionResult> Details(int id,ItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = await GetUserId();
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LikeComment(int id,int itemId)
        {
            var userId = await GetUserId();
            var userName = await GetUserName();
            var like = await _context.LikeOnComment.FirstOrDefaultAsync(i => i.CommentId == id && i.UserId == userId);

            if (like == null)
                DbObjects.LikeComment(_context, id, userId,userName);
            else
                ChangeLikeState(like);

            return RedirectToAction("Details",new {id = itemId});
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LikeItem(int id)
        {
            var userId = await GetUserId();
            var userName = await GetUserName();
            var like = await _context.LikeOnItem.FirstOrDefaultAsync(i => i.ItemId == id && i.UserId == userId);

            if (like == null)
                DbObjects.LikeItem(_context, id, userId,userName);
            else
                ChangeLikeState(like);

            return RedirectToAction("Details", new {id = id});
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
                .Include(l => l.Likes)
                .Include(i => i.Item)
                .Where(m => m.ItemId == id);

            var likes = _context.LikeOnItem.Where(m => m.ItemId == id);

            var itemViewModel = new ItemViewModel
            {
                Item = item,
                Comments = comments,
                LikesOnItem = likes
            };

            return itemViewModel;
        }

        private async Task<string> GetUserId()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userId = user.Id;
            return userId;
        }

        private async Task<string> GetUserName()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userName = user.UserName;
            return userName;
        }

        private void ChangeLikeState(LikeOnComment like)
        {
            like.IsLiked = like.IsLiked ? like.IsLiked = false : like.IsLiked = true;

            _context.Update(like);
            _context.SaveChanges();
        }

        private void ChangeLikeState(LikeOnItem like)
        {
            like.IsLiked = like.IsLiked ? like.IsLiked = false : like.IsLiked = true;

            _context.Update(like);
            _context.SaveChanges();
        }
    }
}
