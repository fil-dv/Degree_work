using Domain.Abstract;
using Domain.Concrete;
using Domain.Entities;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
            AddBindings();
        }

        private void AddBindings()
        {
            //Mock<IBookRepository> mock = new Mock<IBookRepository>();
            //mock.Setup(m => m.Books).Returns(new List<Book>
            //{
            //    new Book {Name = "книга 1", Author = "автор 1", Price = 100},
            //    new Book { Name = "книга 2", Author = "автор 2", Price = 200 },
            //    new Book { Name = "книга 3", Author = "автор 3", Price = 300 }
            //});
            //kernel.Bind<IBookRepository>().ToConstant(mock.Object);

            kernel.Bind<IBookRepository>().To<EFBookRepository>();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);                
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}