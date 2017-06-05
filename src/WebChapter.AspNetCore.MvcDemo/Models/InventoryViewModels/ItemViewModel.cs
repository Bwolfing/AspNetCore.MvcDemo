using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebChapter.AspNetCore.MvcDemo.Models.InventoryViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Sku { get; set; }

        public int Supply { get; set; }

        public ItemViewModel()
        {
        }

        public ItemViewModel(Item i)
        {
            Id = i.Id;
            Name = i.Name;
            Sku = i.Sku;
            Supply = i.Supply;
        }
    }
}
