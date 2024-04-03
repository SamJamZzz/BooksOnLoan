using Microsoft.AspNetCore.Mvc;
using BooksOnLoan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BooksOnLoan.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<CustomRole> _roleManager;

        public UsersController(UserManager<CustomUser> userManager, RoleManager<CustomRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Users/RegisteredUsers
        public async Task<IActionResult> RegisteredUsers()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var usersWithNoRole = allUsers.Where(u => !_userManager.GetRolesAsync(u).Result.Any()).ToList();

            return View(usersWithNoRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AuthorizeMemberRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            if (!await _userManager.IsInRoleAsync(user, "Member"))
            {
                var result = await _userManager.AddToRoleAsync(user, "Member");
                if (!result.Succeeded)
                {
                    return BadRequest();
                }
            }

            return RedirectToAction(nameof(RegisteredUsers));
        }
    }
}
