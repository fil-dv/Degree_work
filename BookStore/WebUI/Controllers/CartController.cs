﻿using Domain.Abstract;
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
        public CartController(IBookRepository repo)
        {
            _repository = repo;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel {
                 Cart = GetCart(),
                 ReturnUrl = returnUrl
            } );
        }


        public Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        
        public RedirectToRouteResult AddToСart(int bookID, string returnUrl)
        {
            Book book = _repository.Books.FirstOrDefault(b => b.BookID == bookID);

            if (book != null)
            {
                GetCart().AddItem(book, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int bookID, string returnUrl)
        {
            Book book = _repository.Books.FirstOrDefault(b => b.BookID == bookID);

            if (book != null)
            {
                GetCart().RemoveLine(book);
            }

            return RedirectToAction("Index", returnUrl);
        }

    }
}