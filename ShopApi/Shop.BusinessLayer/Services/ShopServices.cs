using Shop.BusinessLayer.Interfaces;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Shop.BusinessLayer.Services
{
    public class ShopServices : IShopServices
    {
        readonly IServiceProvider _serviceProvider;
        public ShopServices(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public T GetService<T>() => _serviceProvider.GetRequiredService<T>();
    }
}
