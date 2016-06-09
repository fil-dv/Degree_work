using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cart
    {
        List<CartLine> _lineCollection = new List<CartLine>();

        public IEnumerable<CartLine> Lines {get { return _lineCollection; } }

        public void AddItem(Book book, int quantity)
        {
            CartLine line = _lineCollection.Where(b => b.Book.BookID == book.BookID).FirstOrDefault();

            if (line == null)
            {
                _lineCollection.Add(new CartLine { Book = book, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Book book)
        {
            _lineCollection.RemoveAll(l => l.Book.BookID == book.BookID);
        }

        public decimal ComputeTotalValue()
        {
            return _lineCollection.Sum(b => b.Book.Price * b.Quantity);
        }

        public void Clear()
        {
            _lineCollection.Clear();
        }


    }
}
