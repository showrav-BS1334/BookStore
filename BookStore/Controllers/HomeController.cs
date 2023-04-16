using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        // database with necessary tables
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        // action for home page ------------------------------------------------------------
        public IActionResult Index()
        {
            IEnumerable<Book> objBook = _db.Books; // books table ta fetch kore anlam
            return View(objBook);
        }

        // for showing book details ----------------------------------------------------------
        public IActionResult ShowBookDetails(int? id)
        {
            var book = _db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            var author = _db.Authors.Find(book.AuthorId);
            if (author == null)
            {
                return NotFound();
            }
            ViewBag.author = author;
            ViewBag.book = book;
            return View();
        }

        // for updating book details --------------------------------------------------------
        // GET
        public IActionResult UpdateBook(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var book = _db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            var author = _db.Authors.Find(book.AuthorId);
            if (author == null)
            {
                return NotFound();
            }
            ViewBag.author = author;
            ViewBag.book = book;
            return View();
        }


        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateBook(Book obj)
        {
            if (ModelState.IsValid)
            {
                _db.Books.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // for removing a book ---------------------------------------------------------
        public IActionResult DeleteBook(int? id)
        {
            var obj = _db.Books.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Books.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}