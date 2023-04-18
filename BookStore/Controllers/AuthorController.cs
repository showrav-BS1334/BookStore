using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookStore.Controllers
{
    public class AuthorController : Controller
    {
        // database with necessary tables
        private readonly ApplicationDbContext _db;
        public AuthorController(ApplicationDbContext db)
        {
            _db = db;
        }

        // action for Author page
        public IActionResult Index()
        {
            return View();
        }

        // for showing author details
        public IActionResult ShowAuthorDetails(int? authorId)
        {
            //using LinQ
            //var obj = (from c in _db.Books
            //                 where c.AuthorId == 3
            //                 select c).ToList();

            var booksOfAuthor = _db.Books.Where(book => book.AuthorId.Equals(authorId)).ToList();
            var aboutAuthor = _db.Authors.Find(authorId);

            if (booksOfAuthor == null || aboutAuthor == null)
            {
                return NotFound();
            }

            ViewBag.booksOfAuthor = booksOfAuthor;
            ViewBag.aboutAuthor = aboutAuthor;
            return View();
        }

        // for updating author details 
        // GET
        public IActionResult UpdateAuthor(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var authorFromDb = _db.Authors.Find(id);
            if (authorFromDb == null)
            {
                return NotFound();
            }
            return View(authorFromDb);
        }


        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateAuthor(Author obj)
        {
            if (ModelState.IsValid)
            {
                _db.Authors.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(obj);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // for removing an author
        public IActionResult DeleteAuthor(int? id)
        {
            // author remove
            var obj = _db.Authors.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Authors.Remove(obj);
            _db.SaveChanges();

            // his all books remove
            var booksOfThisAuthor = _db.Books.Where(book => book.AuthorId.Equals(id)).ToList();
            foreach (var book in booksOfThisAuthor)
            {
                _db.Books.Remove(book);
                _db.SaveChanges();
            }
            //_db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


        // for adding a new author to author table
        // GET
        public IActionResult AddAuthor()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAuthor(Author author)
        {
            if (ModelState.IsValid)
            {
                _db.Authors.Add(author);
                _db.SaveChanges();
                return RedirectToAction("Addbook", "Book");
            }
            return View();
        }
    }
}