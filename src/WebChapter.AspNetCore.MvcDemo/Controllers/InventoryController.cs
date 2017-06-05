using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebChapter.AspNetCore.MvcDemo.Data.DAL;
using WebChapter.AspNetCore.MvcDemo.Models;
using WebChapter.AspNetCore.MvcDemo.Models.InventoryViewModels;

namespace WebChapter.AspNetCore.MvcDemo.Controllers
{
    public class InventoryController : Controller
    {
        private const int PageSize = 5;

        private readonly IInventory _inventory;

        public InventoryController(IInventory inventory)
        {
            _inventory = inventory;
        }

        public IActionResult Index(int page = 1)
        {
            if (page <= 0)
            {
                return RedirectToAction(nameof(Index), new {page = 1});
            }

            var viewModel = GetPaginatedInventory(page);

            if (viewModel.LastPage == 0 && page != 1)
            {
                return RedirectToAction(nameof(Index), new {page = 1});
            }

            if (viewModel.LastPage < page)
            {
                return RedirectToAction(nameof(Index), new {page = viewModel.LastPage});
            }

            return View(viewModel);
        }

        private IPaginatedViewModel<ItemViewModel> GetPaginatedInventory(int currentPage)
        {
            var zeroIndexedPage = currentPage - 1;

            var allItems = _inventory.GetItems();
            var itemsForPage = allItems.Skip(PageSize * zeroIndexedPage).Take(PageSize);

            return new PaginatedViewModel<ItemViewModel>
            {
                CurrentPage = currentPage,
                LastPage = (int)Math.Ceiling(allItems.Count() / (double)PageSize),
                Models = new IndexableEnumerable<ItemViewModel>(
                    itemsForPage.Select(i => new ItemViewModel(i))
                ),
                PagerUrl = Url.Action(nameof(Index))
            };
        }
    }
}
