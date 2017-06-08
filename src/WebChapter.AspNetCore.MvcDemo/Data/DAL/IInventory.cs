using System.Linq;
using System.Threading.Tasks;
using WebChapter.AspNetCore.MvcDemo.Models;

namespace WebChapter.AspNetCore.MvcDemo.Data.DAL
{
    public interface IInventory
    {
        IQueryable<Item> GetItems();
        IQueryable<Item> GetAdultItems();
        IQueryable<Item> GetAlcoholItems();
        Task<Item> GetItemById(int id);
    }
}
