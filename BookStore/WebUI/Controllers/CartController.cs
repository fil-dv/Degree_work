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
    public class CartController : Controller
    {
        IBookRepository _repository;
        IOrderProcessor _processor; 

        public CartController(IBookRepository repo, IOrderProcessor processor)
        {
            _repository = repo;
            _processor = processor;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel {
                 Cart = cart,
                 ReturnUrl = returnUrl
            } );
        }

        
        
        public RedirectToRouteResult AddToСart(Cart cart, int bookID, string returnUrl)
        {
            Book book = _repository.Books.FirstOrDefault(b => b.BookID == bookID);

            if (book != null)
            {
                cart.AddItem(book, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int bookID, string returnUrl)
        {
            Book book = _repository.Books.FirstOrDefault(b => b.BookID == bookID);

            if (book != null)
            {
                cart.RemoveLine(book);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult CheckOut()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult CheckOut(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, корзина пуста!");
            }

            if (ModelState.IsValid)
            {
                _processor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(new ShippingDetails());
            }
        }

    }
}