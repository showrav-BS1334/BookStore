using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        // database with necessary tables
        private readonly ApplicationDbContext _db;
        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }

        // action for Book page
        public IActionResult Index()
        {
            return View();
        }

        // for showing book details
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

        // for updating book details
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
                return RedirectToAction("Index", "Home");
            }
            return View(obj);
        }

        // for removing a book
        public IActionResult DeleteBook(int? id)
        {
            var obj = _db.Books.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Books.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        // for adding a new book to book table
        // GET
        public IActionResult AddBook()
        {
            ViewBag.authors = _db.Authors;
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBook(Book book)
        {
            if (ModelState.IsValid)
            {
                _db.Books.Add(book);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(book);
        }

        //public IActionResult testMethod()
        //{
        //    return RedirectToAction("Index");
        //}
    }
}