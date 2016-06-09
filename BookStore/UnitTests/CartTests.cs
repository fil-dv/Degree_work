using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

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
            List<CartLine> result = cart.Lines.OrderBy(b=>b.Book.BookID).ToList();

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
            Assert.IsTrue(cart.Lines.Where(b=>b.Book == book2).Count() == 0);
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
    }
}
