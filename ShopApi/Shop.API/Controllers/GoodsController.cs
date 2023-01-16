using Microsoft.AspNetCore.Mvc;
using Shop.BusinessLayer.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shop.API.Models;
using Shop.BusinessLayer.Models.Goods.ResponseModels;

namespace Shop.API.Controllers
{
    public class GoodsController : ShopBaseController
    {
        private readonly IGoods _goods;

        public GoodsController(IShopServices services) : base(services)
        {
            _goods = services.GetService<IGoods>();
        }

        [HttpGet]
        [Route("goods")]
        public async Task<ResponseObjectModel<List<GoodsResponseModel>>> GetGoods()
        {
            ResponseObjectModel<List<GoodsResponseModel>> responseObject = new ResponseObjectModel<List<GoodsResponseModel>>();
            var allGoods = await _goods.GetGoodsAsync();
            responseObject.Data =  Mapper.Map<List<GoodsResponseModel>>(allGoods);
            return responseObject;
        }

        [HttpGet]
        [Route("goods/{article}")]
        public async Task<ResponseObjectModel<GoodsResponseModel>> GetGoodsByArticle(byte article)
        {
            ResponseObjectModel<GoodsResponseModel> responseObject = new ResponseObjectModel<GoodsResponseModel>();
            var goods = await _goods.GetGoodsByArticleAsync(article);
            responseObject.Data = Mapper.Map<GoodsResponseModel>(goods);
            return responseObject;
        }
    }
}