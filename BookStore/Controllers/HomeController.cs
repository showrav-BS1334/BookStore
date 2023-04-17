using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Book> objBook = _db.Books;
            return View(objBook);
        }

        /*public IActionResult testMethod()
        {
            return RedirectToAction("Index");

        }*/
    }
}