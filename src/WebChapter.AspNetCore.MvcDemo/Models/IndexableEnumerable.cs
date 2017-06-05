using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebChapter.AspNetCore.MvcDemo.Models
{
    public class IndexableEnumerable<T> : IIndexableEnumerable<T>
    {
        private readonly List<T> _items = new List<T>();

        public IndexableEnumerable(IEnumerable<T> enumerable)
        {
            _items = enumerable.ToList();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public T this[int index] => _items[index];
    }
}
