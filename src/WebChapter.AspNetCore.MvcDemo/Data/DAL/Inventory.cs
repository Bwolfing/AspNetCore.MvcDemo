using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebChapter.AspNetCore.MvcDemo.Models;
using WebChapter.AspNetCore.MvcDemo.Options;

namespace WebChapter.AspNetCore.MvcDemo.Data.DAL
{
    public class Inventory : IInventory
    {
        private readonly List<Item> _adultItems = new List<Item>();
        private readonly List<Item> _alcoholItems = new List<Item>();
        private readonly List<Item> _items = new List<Item>();

        public Inventory(IOptionsSnapshot<InventoryOptions> options)
        {
            var random = new Random(DateTime.Now.Millisecond);
            for (var i = 1; i <= options.Value.InventoryRepoSize; i++)
            {
                _items.Add(new Item
                {
                    Id = i,
                    Name = $"Product {i}",
                    Sku = i.ToString(),
                    Supply = random.Next() % 50
                });
            }

            for (var i = 1; i <= options.Value.InventoryRepoSize; i++)
            {
                _adultItems.Add(new Item
                {
                    Id = i + _items.Count,
                    Name = $"Adult Product {i}",
                    Sku = (i + _items.Count).ToString(),
                    Supply = random.Next() % 50
                });
            }

            for (var i = 1; i <= options.Value.InventoryRepoSize; i++)
            {
                _alcoholItems.Add(new Item
                {
                    Id = i + (options.Value.InventoryRepoSize * 2),
                    Name = $"Alcohol Product {i}",
                    Sku = (i + (options.Value.InventoryRepoSize * 2)).ToString(),
                    Supply = random.Next() % 50
                });
            }
        }

        public IQueryable<Item> GetItems()
        {
            return _items.AsQueryable();
        }

        public IQueryable<Item> GetAdultItems()
        {
            return _adultItems.AsQueryable();
        }

        public IQueryable<Item> GetAlcoholItems()
        {
            return _alcoholItems.AsQueryable();
        }

        public Task<Item> GetItemById(int id)
        {
            return GetItems().FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
