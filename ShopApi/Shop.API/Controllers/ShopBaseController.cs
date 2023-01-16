using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shop.BusinessLayer.Interfaces;

namespace Shop.API.Controllers
{
    public class ShopBaseController : ControllerBase
    {
        private readonly IShopServices _services;
        protected IMapper Mapper => _services.GetService<IMapper>();
        public ShopBaseController(IShopServices services)
        {
            this._services = services;
        }
    }
}
