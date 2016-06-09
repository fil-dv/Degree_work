using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using Domain.Abstract;
using Moq;
using System.Collections.Generic;
using WebUI.Controllers;
using System.Collections;
using System.Linq;
using WebUI.HtmlHelpers;
using System.Web.Mvc;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
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

            BooksListViewModel result = (BooksListViewModel)controller.List(null, 3).Model;

            List<Book> books = result.Books.ToList();

            Assert.IsTrue(books.Count == 3);
            Assert.AreEqual(books[0].Name, "книга 7");
            Assert.AreEqual(books[1].Name, "книга 8");
            Assert.AreEqual(books[2].Name, "книга 9");
        }

        //[TestMethod]
        //public void Can_Generate_Page_Links()
        //{
        //    //Организация, имитируем контекст
        //    HtmlHelper myHelper = null;

        //    PagingInfo pagingInfo = new PagingInfo
        //    {
        //        CurrentPage = 2,
        //        TotalItems = 28,
        //        ItemPerPage = 10
        //    };
            
        //    Func<int, string> PageUrlDelegat = i => "Page" + i;

        //    //непосредственный вызов
        //    MvcHtmlString result = myHelper.PageLinks(pagingInfo, PageUrlDelegat);

        //    //тестим соответствует ли ожидаемое тому, что получили
        //    Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
        //        + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
        //        + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
        //        result.ToString());
        //}

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
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
            controller.pageSize = 2;

            // Действие (act)
            BooksListViewModel result = (BooksListViewModel)controller.List(null, 2).Model;

            //утверждения
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemPerPage, 2);
            Assert.AreEqual(pagingInfo.TotalItems, 9);
            Assert.AreEqual(pagingInfo.TotalPages, 5);
        }

        [TestMethod]
        public void Can_Filter_Books()
        {
            // Организация (arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book {BookID = 1, Name = "книга 1", Author = "автор 1", Price = 100, Genre = "Genre 1"},
                new Book {BookID = 2, Name = "книга 2", Author = "автор 2", Price = 200, Genre = "Genre 2"}, 
                new Book {BookID = 3, Name = "книга 3", Author = "автор 3", Price = 300, Genre = "Genre 3"},
                new Book {BookID = 4, Name = "книга 4", Author = "автор 4", Price = 400, Genre = "Genre 3"},
                new Book {BookID = 5, Name = "книга 5", Author = "автор 5", Price = 500, Genre = "Genre 5"},
                new Book {BookID = 6, Name = "книга 6", Author = "автор 6", Price = 600, Genre = "Genre 6"},
                new Book {BookID = 7, Name = "книга 7", Author = "автор 7", Price = 700, Genre = "Genre 7"},
                new Book {BookID = 8, Name = "книга 8", Author = "автор 8", Price = 800, Genre = "Genre 3"},
                new Book {BookID = 9, Name = "книга 9", Author = "автор 9", Price = 900, Genre = "Genre 3"}
            });

            BooksController controller = new BooksController(mock.Object);
            controller.pageSize = 2;

            // Действие (act)
            List<Book> result = ((BooksListViewModel)controller.List(null, 1).Model).Books.ToList();

            //утверждения

            //Assert.AreEqual(result.Count, 2);
            Assert.IsTrue(result[0].Name == "книга 1" && result[0].Genre == "Genre 1");
            Assert.IsTrue(result[1].Name == "книга 2" && result[1].Genre == "Genre 2");
            //Assert.AreEqual(result[1].Name, "книга 9");            
        }

        [TestMethod]
        public void Can_Create_Genre_List()
        {
            // Организация (arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book {BookID = 1, Name = "книга 1", Author = "автор 1", Price = 100, Genre = "Genre 1"},
                new Book {BookID = 2, Name = "книга 2", Author = "автор 2", Price = 200, Genre = "Genre 2"},
                new Book {BookID = 3, Name = "книга 3", Author = "автор 3", Price = 300, Genre = "Genre 3"},
                new Book {BookID = 4, Name = "книга 4", Author = "автор 4", Price = 400, Genre = "Genre 3"},
                new Book {BookID = 5, Name = "книга 5", Author = "автор 5", Price = 500, Genre = "Genre 5"},
                new Book {BookID = 6, Name = "книга 6", Author = "автор 6", Price = 600, Genre = "Genre 6"},
                new Book {BookID = 7, Name = "книга 7", Author = "автор 7", Price = 700, Genre = "Genre 7"},
                new Book {BookID = 8, Name = "книга 8", Author = "автор 8", Price = 800, Genre = "Genre 3"},
                new Book {BookID = 9, Name = "книга 9", Author = "автор 9", Price = 900, Genre = "Genre 3"}
            });

            NavController controller = new NavController(mock.Object);
            

            // Действие (act)
            List<string> result = ((IEnumerable<string>)(controller.Menu().Model)).ToList();

            //утверждения

            Assert.IsTrue(result.Count == 6);
            Assert.AreEqual(result[0], "Genre 1");
            Assert.AreEqual(result[1], "Genre 2");
            Assert.AreEqual(result[2], "Genre 3");
            Assert.AreEqual(result[3], "Genre 5");
            Assert.AreEqual(result[4], "Genre 6");
            Assert.AreEqual(result[5], "Genre 7");
        }

        [TestMethod]
        public void Can_Indicate_Selected_Genre()
        {
            // Организация (arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book {BookID = 1, Name = "книга 1", Author = "автор 1", Price = 100, Genre = "Genre 1"},
                new Book {BookID = 2, Name = "книга 2", Author = "автор 2", Price = 200, Genre = "Genre 2"},
                new Book {BookID = 3, Name = "книга 3", Author = "автор 3", Price = 300, Genre = "Genre 3"},
                new Book {BookID = 4, Name = "книга 4", Author = "автор 4", Price = 400, Genre = "Genre 3"},
                new Book {BookID = 5, Name = "книга 5", Author = "автор 5", Price = 500, Genre = "Genre 5"},
                new Book {BookID = 6, Name = "книга 6", Author = "автор 6", Price = 600, Genre = "Genre 6"},
                new Book {BookID = 7, Name = "книга 7", Author = "автор 7", Price = 700, Genre = "Genre 7"},
                new Book {BookID = 8, Name = "книга 8", Author = "автор 8", Price = 800, Genre = "Genre 3"},
                new Book {BookID = 9, Name = "книга 9", Author = "автор 9", Price = 900, Genre = "Genre 3"}
            });

            NavController controller = new NavController(mock.Object);
            
            // Действие (act)
            string result = controller.Menu("Genre 3").ViewBag.SelectedGenre;

            //утверждения

            Assert.AreEqual(result, "Genre 3");
        }

        [TestMethod]
        public void Can_Get_Count_TotalItems_For_Any_Genre()
        {
            // Организация (arrange)
            Mock mock = GetData();

            BooksController controller = new BooksController((IBookRepository)mock.Object);

            // Действие (act)
            int result1 = ((BooksListViewModel)controller.List("Genre 1").Model).PagingInfo.TotalItems;
            int result2 = ((BooksListViewModel)controller.List("Genre 2").Model).PagingInfo.TotalItems;
            int result3 = ((BooksListViewModel)controller.List("Genre 3").Model).PagingInfo.TotalItems;
            int result7 = ((BooksListViewModel)controller.List("Genre 7").Model).PagingInfo.TotalItems;
            int resultAll = ((BooksListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            //утверждения

            Assert.AreEqual(result1, 1);
            Assert.AreEqual(result2, 1);
            Assert.AreEqual(result3, 4);
            Assert.AreEqual(result7, 3);
            Assert.AreEqual(resultAll, 9);
        }

    }
}
