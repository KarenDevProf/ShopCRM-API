using Microsoft.EntityFrameworkCore;
using Shop.BusinessLayer.Interfaces;
using Shop.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Repositories.Repositories.Interfaces;
using Shop.BusinessLayer.Exceptions;

namespace Shop.BusinessLayer.Services
{
    public class GoodsBl : IGoods
    {
        private readonly IRepository<Good> _good;
        public GoodsBl(IRepository<Good> good)
        {
            _good = good;
        }

        public async Task<List<Good>> GetGoodsAsync()
        {
            return await _good.GetAllAsync();
        }

        public async Task<Good> GetGoodsByArticleAsync(byte article)
        {
            var goods = await _good.FindBy(e => e.Article == article).FirstOrDefaultAsync();
            
            if (goods == null) {
                throw new NotFoundException("Goods"); 
            }

            return goods;
        }
    }
}
