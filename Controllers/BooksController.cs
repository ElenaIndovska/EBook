using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EBook.Data;
using EBook.Models;
using EBook.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace EBook.Controllers
{
    public class BooksController : Controller
    {
        private readonly EBookContext _context;

        public BooksController(EBookContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string bookGenre, string searchString)
        {
            //return View(await _context.Book.ToListAsync());
            IQueryable<Book> books = _context.Book.AsQueryable();
            IQueryable<string> bookGenreQuery = _context.Book.OrderBy(c => c.Genre).Select(c => c.Genre).Distinct();

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(c => c.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(bookGenre))
            {
                books = books.Where(c => c.Genre == bookGenre);
            }

            books = books.Include(m => m.Authors).ThenInclude(m => m.Author);

            var bookVM = new BookViewModel
            {
                Genres = new SelectList(await bookGenreQuery.ToListAsync()),
                Books = await books.ToListAsync()
            };

            return View(bookVM);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(m => m.Authors).ThenInclude(m => m.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file, [Bind("Id,Title,Genre,BookQuantity,Price,Year,ImgFile")] Book book)
        {
            if (file != null)
            {
                string filename = file.FileName;
                //  string  ext = Path.GetExtension(file.FileName);
                string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images"));
                using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                { await file.CopyToAsync(filestream); }

                book.ImgFile = filename;
            }


            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile file, int id, [Bind("Id,Title,Genre,BookQuantity,Price,Year,ImgFile")] Book book)
        {
            if (file != null)
            {
                string filename = file.FileName;
                //  string  ext = Path.GetExtension(file.FileName);
                string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images"));
                using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                { await file.CopyToAsync(filestream); }

                book.ImgFile = filename;
            }

            _context.Update(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }

        [Authorize]        
        [HttpGet]
        public async Task<IActionResult> Download(string id)
        {
            
            var path = @"C:\Users\Indovski\Desktop\EBook\EBook\"+id+".pdf";
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open)){
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return File(memory, GetMimeTypes()[ext], Path.GetFileName(path));
            
        }


        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".pdf", "application/pdf" }
            };
        }
    }
}
