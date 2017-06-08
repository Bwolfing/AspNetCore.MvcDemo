using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using WebChapter.AspNetCore.MvcDemo.Data.DAL;
using WebChapter.AspNetCore.MvcDemo.Models;
using WebChapter.AspNetCore.MvcDemo.Models.InventoryViewModels;
using WebChapter.AspNetCore.MvcDemo.Options;
using WebChapter.AspNetCore.MvcDemo.Security;

namespace WebChapter.AspNetCore.MvcDemo.Controllers
{
    [Authorize(AuthorizationPolicies.EmployeeOnly)]
    public class InventoryController : Controller
    {
        private readonly int _pageSize;
        private readonly IInventory _inventory;

        public InventoryController(IInventory inventory, IOptionsSnapshot<InventoryOptions> options)
        {
            _inventory = inventory;
            _pageSize = options.Value.PageSize;
        }

        public IActionResult Index(int page = 1)
        {
            return PaginatedView(_inventory.GetItems, nameof(Index), page);
        }

        // Adult content is for adults, i.e. those 18+.
        [Authorize(AuthorizationPolicies.Over18)]
        [Route("[controller]/adult-content")]
        public IActionResult AdultContent(int page = 1)
        {
            return PaginatedView(_inventory.GetAdultItems, nameof(AdultContent), page);
        }

        // We only want employees over 21 since alcohol is for 21+ individuals.
        [Authorize(AuthorizationPolicies.Over21)]
        public IActionResult Alcohol(int page = 1)
        {
            return PaginatedView(_inventory.GetAlcoholItems, nameof(Alcohol), page);
        }

        private IActionResult PaginatedView(Func<IQueryable<Item>> getItems, string viewName, int page)
        {
            if (page <= 0)
            {
                return RedirectToAction(viewName, new { page = 1 });
            }

            var viewModel = GetPaginatedInventory(getItems, page);

            if (viewModel.LastPage == 0 && page != 1)
            {
                return RedirectToAction(viewName, new { page = 1 });
            }

            if (viewModel.LastPage < page)
            {
                return RedirectToAction(viewName, new { page = viewModel.LastPage });
            }

            return View(viewName, viewModel);
        }

        private IPaginatedViewModel<ItemViewModel> GetPaginatedInventory(Func<IQueryable<Item>> getItems, int currentPage)
        {
            var zeroIndexedPage = currentPage - 1;

            var allItems = getItems();
            var itemsForPage = allItems.Skip(_pageSize * zeroIndexedPage).Take(_pageSize);

            return new PaginatedViewModel<ItemViewModel>
            {
                CurrentPage = currentPage,
                LastPage = (int)Math.Ceiling(allItems.Count() / (double)_pageSize),
                Models = itemsForPage.Select(i => new ItemViewModel(i)),
                PagerUrl = Url.Action(nameof(Index))
            };
        }
    }
}
