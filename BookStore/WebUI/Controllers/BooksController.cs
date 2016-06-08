using Domain.Abstract;
using Domain.Entities;
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
        //private IBookRepository repository;
        //public int pageSize = 2;

        //public BooksController(IBookRepository repo)
        //{
        //    repository = repo;
        //}

        //public ViewResult List(string genre, int page = 1)
        //{
            //IEnumerable<Book> Books = repository.Books.Where(b => b.Genre == null || b.Genre == currentGenre).
            //                            OrderBy(b => b.BookID).
            //                            Skip((page - 1) * pageSize).
            //                            Take(pageSize);


            //    BooksListViewModel model = new BooksListViewModel
            //    {

            //        Books = repository.Books.Where(b => b.Genre == (string)null || b.Genre == genre).
            //                                OrderBy(b => b.BookID).
            //                                Skip((page - 1) * pageSize).
            //                                Take(pageSize),
            //        PagingInfo = new PagingInfo {

            //            CurrentPage = page,
            //            ItemPerPage = pageSize,
            //            TotalItems = repository.Books.Count()
            //        },
            //        CurrentGenre = genre
            //    };

            //    return View(model);
            //}

        private IBookRepository repository;
        public int pageSize = 3;

        public BooksController(IBookRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string genre, int page = 1)
        {
            BooksListViewModel model = new BooksListViewModel
            {
                Books = repository.Books
                .Where(b => genre == null || b.Genre == genre)
                .OrderBy(book => book.BookID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemPerPage = pageSize,
                    TotalItems = genre == null ?
                        repository.Books.Count() :
                        repository.Books.Where(book => book.Genre == genre).Count()
                },
                CurrentGenre = genre
            };

            return View(model);
        }
    }
}