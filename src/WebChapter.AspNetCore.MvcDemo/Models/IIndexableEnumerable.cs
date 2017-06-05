using System.Collections.Generic;

namespace WebChapter.AspNetCore.MvcDemo.Models
{
    public interface IIndexableEnumerable<T> : IEnumerable<T>
    {
        T this[int index] { get; }
    }
}
