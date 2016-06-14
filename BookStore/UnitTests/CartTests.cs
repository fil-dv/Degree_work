using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using WebUI.Controllers;
using Moq;
using Domain.Abstract;
using System.Web.Mvc;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //arrange
            Book book1 = new Book { BookID = 1, Name = "book_1" };
            Book book2 = new Book { BookID = 2, Name = "book_2" };
            Book book3 = new Book { BookID = 3, Name = "book_3" };
            Cart cart = new Cart();

            //act
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 2);
            cart.AddItem(book3, 3);
            List<CartLine> result = cart.Lines.ToList();

            //assert
            Assert.AreEqual(result.Count, 3);
            Assert.IsTrue(result[0].Book.Name == "book_1" && result[0].Quantity == 1);
            Assert.IsTrue(result[1].Book.Name == "book_2" && result[1].Quantity == 2);
            Assert.IsTrue(result[2].Book.Name == "book_3" && result[2].Quantity == 3);
        }

        [TestMethod]
        public void Can_Add_Quantity_To_Exist_Line()
        {
            //arrange
            Book book1 = new Book { BookID = 1, Name = "book_1" };
            Book book2 = new Book { BookID = 2, Name = "book_2" };
            Book book3 = new Book { BookID = 3, Name = "book_3" };
            Cart cart = new Cart();

            //act
            cart.AddItem(book3, 3);
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 2);
            cart.AddItem(book3, 4);
            cart.AddItem(book1, 12);
            List<CartLine> result = cart.Lines.OrderBy(b => b.Book.BookID).ToList();

            //assert
            Assert.AreEqual(result.Count, 3);
            Assert.IsTrue(result[0].Book.Name == "book_1" && result[0].Quantity == 13);
            Assert.IsTrue(result[1].Book.Name == "book_2" && result[1].Quantity == 2);
            Assert.IsTrue(result[2].Book.Name == "book_3" && result[2].Quantity == 7);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            //arrange
            Book book1 = new Book { BookID = 1, Name = "book_1" };
            Book book2 = new Book { BookID = 2, Name = "book_2" };
            Book book3 = new Book { BookID = 3, Name = "book_3" };
            Cart cart = new Cart();

            //act
            cart.AddItem(book3, 3);
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 2);
            cart.AddItem(book3, 4);
            cart.AddItem(book1, 12);
            cart.RemoveLine(book2);
            List<CartLine> result = cart.Lines.OrderBy(b => b.Book.BookID).ToList();

            //assert
            Assert.AreEqual(result.Count, 2);
            Assert.IsTrue(result[0].Book.Name == "book_1" && result[0].Quantity == 13);
            Assert.IsTrue(result[1].Book.Name == "book_3" && result[1].Quantity == 7);
            Assert.IsTrue(cart.Lines.Where(b => b.Book == book2).Count() == 0);
        }

        [TestMethod]
        public void Can_Calculate_Total_Price()
        {
            //arrange
            Book book1 = new Book { BookID = 1, Name = "book_1", Price = 50 };
            Book book2 = new Book { BookID = 2, Name = "book_2", Price = 200 };
            Book book3 = new Book { BookID = 3, Name = "book_3", Price = 300 };
            Cart cart = new Cart();

            //act
            cart.AddItem(book3, 1);
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 2);

            decimal result = cart.ComputeTotalValue();

            //assert
            Assert.AreEqual(result, 750);
        }


        [TestMethod]
        public void Can_Clean_List()
        {
            //arrange
            Book book1 = new Book { BookID = 1, Name = "book_1" };
            Book book2 = new Book { BookID = 2, Name = "book_2" };
            Book book3 = new Book { BookID = 3, Name = "book_3" };
            Cart cart = new Cart();

            //act
            cart.AddItem(book3, 3);
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 2);
            cart.AddItem(book3, 4);
            cart.AddItem(book1, 12);
            cart.Clear();
            List<CartLine> result = cart.Lines.OrderBy(b => b.Book.BookID).ToList();

            //assert
            Assert.AreEqual(result.Count, 0);
        }

        Mock<IBookRepository> GetData()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book {BookID = 1, Name = "книга 1", Author = "автор 1", Price = 100, Genre = "Genre 1"},
                new Book {BookID = 2, Name = "книга 2", Author = "автор 2", Price = 200, Genre = "Genre 2"},
                new Book {BookID = 3, Name = "книга 3", Author = "автор 3", Price = 300, Genre = "Genre 3"},
                new Book {BookID = 4, Name = "книга 4", Author = "автор 4", Price = 400, Genre = "Genre 7"},
                new Book {BookID = 5, Name = "книга 5", Author = "автор 5", Price = 500, Genre = "Genre 3"},
                new Book {BookID = 6, Name = "книга 6", Author = "автор 6", Price = 600, Genre = "Genre 7"},
                new Book {BookID = 7, Name = "книга 7", Author = "автор 7", Price = 700, Genre = "Genre 7"},
                new Book {BookID = 8, Name = "книга 8", Author = "автор 8", Price = 800, Genre = "Genre 3"},
                new Book {BookID = 9, Name = "книга 9", Author = "автор 9", Price = 900, Genre = "Genre 3"}
            });
            return mock;
        }


        [TestMethod]
        public void Can_Add_To_Cart_Use_ModelBinder()
        {
            //arrange
            Mock<IBookRepository> mock = GetData();
            CartController controller = new CartController(mock.Object, null);
            Cart cart = new Cart();

            //act
            controller.AddToСart(cart, 1, null);

            //assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.IsTrue(cart.Lines.ToList()[0].Book.BookID == 1);

        }

        [TestMethod]
        public void Can_Build_Right_Url_After_Add_To_Cart()
        {
            //arrange
            Mock<IBookRepository> mock = GetData();
            CartController controller = new CartController(mock.Object, null);
            Cart cart = new Cart();

            //act
            RedirectToRouteResult result = controller.AddToСart(cart, 1, "myUrl");

            //assert
            Assert.AreEqual(result.RouteValues["Action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_Method_Index_Build_Right_Url()
        {
            //arrange
            CartController controller = new CartController(null, null);
            Cart cart = new Cart();

            //act
            CartIndexViewModel result = (CartIndexViewModel)controller.Index(cart, "myUrl").ViewData.Model;

            //assert
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();

            CartController controller = new CartController(null, mock.Object);

            ViewResult result = controller.CheckOut(cart, shippingDetails);

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }


        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);

            CartController controller = new CartController(null, mock.Object);
            controller.ModelState.AddModelError("error", "error");

            ViewResult result = controller.CheckOut(cart, new ShippingDetails());

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_And_Submit_Order()
        {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);

            CartController controller = new CartController(null, mock.Object);

            ViewResult result = controller.CheckOut(cart, new ShippingDetails());

            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());

            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }

    }
}



