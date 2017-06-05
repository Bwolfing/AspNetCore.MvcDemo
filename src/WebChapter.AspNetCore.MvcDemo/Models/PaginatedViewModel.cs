namespace WebChapter.AspNetCore.MvcDemo.Models
{
    public class PaginatedViewModel<TViewModel> : IPaginatedViewModel<TViewModel>
    {
        public int CurrentPage { get; set; }

        public int LastPage { get; set; }

        public string PagerUrl { get; set; }

        public IIndexableEnumerable<TViewModel> Models { get; set; }
    }
}
