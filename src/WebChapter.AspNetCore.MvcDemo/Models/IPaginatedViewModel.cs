namespace WebChapter.AspNetCore.MvcDemo.Models
{
    public interface IPaginatedViewModel<TViewModel>
    {
        int CurrentPage { get; }

        int LastPage { get; }

        string PagerUrl { get; }

        IIndexableEnumerable<TViewModel> Models { get; }
    }
}
