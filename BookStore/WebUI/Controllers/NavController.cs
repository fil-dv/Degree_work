using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {
        IBookRepository _repository;

        public NavController(IBookRepository repo)
        {
            _repository = repo;
        }

        // GET: Nav
        public PartialViewResult Menu(string genre = null)
        {
            ViewBag.SelectedGenre = genre;
            IEnumerable<string> genres = _repository.Books.
                                                    Select(book => book.Genre).
                                                    Distinct().
                                                    OrderBy(x => x);

            return PartialView(genres);
        }
    }
}