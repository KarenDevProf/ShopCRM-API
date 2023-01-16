using Shop.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.BusinessLayer.Interfaces
{
    public interface IGoods
    {
        Task<Good> GetGoodsByArticleAsync(byte article);
        Task<List<Good>> GetGoodsAsync();
    }
}