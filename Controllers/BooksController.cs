using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BooksOnLoan.Data;
using BooksOnLoan.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BooksOnLoan.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly BookContext _context;

        public BooksController(BookContext context)
        {
            _context = context;
        }

        // GET: Books
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pastDueHolds = await _context.Holds
                .Where(h => h.UserId == userId && h.DueDate < DateTime.Now && !h.IsReturned)
                .ToListAsync();

            if (pastDueHolds.Any())
            {
                TempData["ShowPastDueAlert"] = true;
            }

            return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
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

            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("BookCode,Author,Title,YearPublished,Quantity")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
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
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("BookCode,Author,Title,YearPublished,Quantity")] Book book)
        {
            if (id != book.BookCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookCode))
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
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookCode == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
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

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookCode == id);
        }
    }
}