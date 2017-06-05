using System.Collections.Generic;

namespace WebChapter.AspNetCore.MvcDemo.Models
{
    public interface IPaginatedViewModel
    {
        int CurrentPage { get; }

        int LastPage { get; }

        string PagerUrl { get; }
    }

    public interface IPaginatedViewModel<TViewModel> : IPaginatedViewModel
    {
        IEnumerable<TViewModel> Models { get; }
    }
}
