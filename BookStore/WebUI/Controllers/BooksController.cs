using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class BooksController : Controller
    {
        private IBookRepository repository;
        public int pageSize = 2;

        public BooksController(IBookRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(int page = 1)
        {
            //return View(repository.Books.
            //            OrderBy(b=>b.BookID).
            //            Skip((page-1)*pageSize).
            //            Take(pageSize));

            BooksListViewModel model = new BooksListViewModel
            {

                Books = repository.Books.OrderBy(b => b.BookID).
                                        Skip((page - 1) * pageSize).
                                        Take(pageSize),
                PagingInfo = new PagingInfo {

                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    TotalItems = repository.Books.Count()
                }
            };

            return View(model);
        }
    }
}