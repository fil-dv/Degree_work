using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using Domain.Abstract;
using Moq;
using System.Collections.Generic;
using WebUI.Controllers;
using System.Collections;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book {Name = "книга 1", Author = "автор 1", Price = 100},
                new Book {Name = "книга 2", Author = "автор 2", Price = 200},
                new Book {Name = "книга 3", Author = "автор 3", Price = 300},
                new Book {Name = "книга 4", Author = "автор 4", Price = 400},
                new Book {Name = "книга 5", Author = "автор 5", Price = 500},
                new Book {Name = "книга 6", Author = "автор 6", Price = 600},
                new Book {Name = "книга 7", Author = "автор 7", Price = 700},
                new Book {Name = "книга 8", Author = "автор 8", Price = 800},
                new Book {Name = "книга 9", Author = "автор 9", Price = 900}
            });
            BooksController controller = new BooksController(mock.Object);
            controller.pageSize = 3;

            IEnumerable<Book> result = (IEnumerable<Book>)controller.List(3).Model;

            List<Book> books = result.ToList();

            Assert.IsTrue(books.Count == 3);
            Assert.AreEqual(books[0].Name, "книга 7");
            Assert.AreEqual(books[1].Name, "книга 8");
            Assert.AreEqual(books[2].Name, "книга 9");
        }
    }
}
