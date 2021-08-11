using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.Infrastructure.Models;
using CourseWork.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CourseWork.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult UserList()
        {
            return View(_userManager.Users.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                RolesViewModel model = new RolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    UserRoles = userRoles,
                    Roles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                await _signInManager.RefreshSignInAsync(user);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }

        private async Task<IActionResult> ChangeUsers(string[] usersIds, Func<User, Task> handler)
        {
            if (usersIds != null)
            {
                var currentUserExist = false;
                foreach (var userId in usersIds)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        await handler(user);
                        var changedUser = await _userManager.FindByIdAsync(user.Id);
                        if (User.Identity.Name == user.UserName && (user.LockoutEnabled || changedUser == null))
                        {
                            currentUserExist = true;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User not found");
                    }
                }
                if (currentUserExist)
                {
                    return RedirectToAction("Logout", "Account");
                }
            }
            return RedirectToAction("UserList", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Block(string[] usersIds)
        {
            return await ChangeUsers(usersIds, async user => {
                user.IsBlocked = true;
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(3);
                await _userManager.UpdateAsync(user);
                await _userManager.UpdateSecurityStampAsync(user);
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unblock(string[] usersIds)
        {
            return await ChangeUsers(usersIds, async user =>
            {
                user.IsBlocked = false;
                user.LockoutEnabled = false;
                user.LockoutEnd = DateTimeOffset.UtcNow;
                await _userManager.UpdateAsync(user);
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string[] usersIds)
        {
            return await ChangeUsers(usersIds, async user =>
            {
                await _userManager.DeleteAsync(user);
            });
        }
    }
}
