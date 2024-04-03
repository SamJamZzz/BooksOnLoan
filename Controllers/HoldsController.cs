using System.Security.Claims;
using BooksOnLoan.Data;
using BooksOnLoan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksOnLoan.Controllers
{
    [Authorize(Roles = "Admin, Member")]
    public class HoldsController : Controller
    {
    private readonly BookContext _context;
        public HoldsController(BookContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrow(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            if (book.Quantity > 0)
            {
                book.Quantity--;
                var hold = new Hold
                {
                    BookCode = book.BookCode,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    HoldDate = DateTime.Now,
                    DueDate = DateTime.Now.AddMonths(1),
                    IsReturned = false
                };

                _context.Holds.Add(hold);
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["Message"] = "Book is not available for borrowing.";
            }
            return RedirectToAction(nameof(Index), "Books");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hold = await _context.Holds.FindAsync(id);
            if (hold == null)
            {
                return NotFound();
            }

            hold.IsReturned = true;
            _context.Holds.Update(hold);

            var book = await _context.Books.FindAsync(hold.BookCode);
            if (book != null)
            {
                book.Quantity++;
                _context.Books.Update(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UserHolds));
        }

        public async Task<IActionResult> UserHolds()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userHolds = await _context.Holds.Include(h => h.Book)
                                                .Where(h => h.UserId == userId)
                                                .ToListAsync();
            return View(userHolds);
        }
    }
}