using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebChapter.AspNetCore.MvcDemo.Models;

namespace WebChapter.AspNetCore.MvcDemo.Data.DAL
{
    public class Inventory : IInventory
    {
        private const int NumItems = 100;

        private readonly List<Item> _items = new List<Item>();

        public Inventory()
        {
            for (var i = 1; i <= NumItems; i++)
            {
                _items.Add(new Item
                {
                    Id = i,
                    Name = $"Product {i}",
                    Sku = i.ToString(),
                    Supply = new Random(DateTime.Now.Millisecond).Next() % 50
                });
            }
        }

        public IQueryable<Item> GetItems()
        {
            return _items.AsQueryable();
        }

        public Task<Item> GetItemById(int id)
        {
            return GetItems().FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
