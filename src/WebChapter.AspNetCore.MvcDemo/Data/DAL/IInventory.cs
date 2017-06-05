using System.Linq;
using System.Threading.Tasks;
using WebChapter.AspNetCore.MvcDemo.Models;

namespace WebChapter.AspNetCore.MvcDemo.Data.DAL
{
    public interface IInventory
    {
        IQueryable<Item> GetItems();
        Task<Item> GetItemById(int id);
    }
}
