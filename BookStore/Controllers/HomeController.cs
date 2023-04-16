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

        // for showing author details -------------------------------------------------
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

        // for updating author details ------------------------------------------------
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
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // for removing an author ----------------------------------------------
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
            return RedirectToAction("Index");
        }

        // for adding a new book to books table ----------------------------------------
        // GET
        public IActionResult AddBook()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBook(Book book, string authorName)
        {
            if (ModelState.IsValid)
            {
                bool isNewAuthor = true;
                var allAuthors = _db.Authors;
                int idPrevAuthor = 0;

                // checking if the author is already here or not
                foreach (var author in allAuthors)
                {
                    if (authorName.Trim() == author.Name)
                    {
                        isNewAuthor = false;
                        idPrevAuthor = author.Id;
                        break;
                    }
                }
                // if we get a new author, update the author table
                if (isNewAuthor)
                {
                    var newAuthor = new Author { Name = authorName };
                    _db.Authors.Add(newAuthor);
                    _db.SaveChanges();
                    var lastObject = _db.Authors.OrderByDescending(x => x.Id).FirstOrDefault();
                    if (lastObject != null)
                    {
                        book.AuthorId = lastObject.Id;
                    }
                }
                else
                {
                    book.AuthorId = idPrevAuthor;
                }

                // update the book table
                _db.Books.Add(book);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        /*        public IActionResult testMethod()
                {
                    return RedirectToAction("Index");

                }*/
    }
}