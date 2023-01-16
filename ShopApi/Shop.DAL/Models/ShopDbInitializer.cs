using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace Shop.DAL.Models
{
    public class ShopDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ShopCRMContext>();

                if (context != null)
                {
                    context.Database.EnsureCreated();

                    if (!context.Users.Any())
                    {
                        context.Users.AddRange(new List<User>()
                        {
                            new User()
                            {
                                FirstName = "John",
                                LastName = "Doe",
                                Age = 25
                            },
                            new User()
                            {
                                FirstName = "Anne",
                                LastName = "Smith",
                                Age = 44
                            },
                            new User()
                            {
                                FirstName = "Sarah",
                                LastName = "Connor",
                                Age = 36
                            }
                        });
                        context.SaveChanges();
                    }

                    if (!context.Goods.Any())
                    {
                        context.Goods.AddRange(new List<Good>()
                        {
                            new Good()
                            {
                                Name = "GoodsName1",
                                Article = 1,
                                Price = 1200
                            },
                            new Good()
                            {
                                Name = "GoodsName2",
                                Article = 2,
                                Price = 4200
                            },
                            new Good()
                            {
                                Name = "GoodsName3",
                                Article = 3,
                                Price = 3200
                            }
                        });
                        context.SaveChanges();
                    }

                    if (!context.Orders.Any())
                    {
                        context.Orders.AddRange(new List<Order>()
                        {
                            new Order()
                            {
                                UserId = 1,
                                OrderNumber = 1,
                                Status = 1,
                                OrderCreateDate = DateTime.Now,
                                OrderUpdateDate = DateTime.Now,
                            },
                            new Order()
                            {
                                UserId = 1,
                                OrderNumber = 2,
                                Status = 1,
                                OrderCreateDate = DateTime.Now,
                                OrderUpdateDate = DateTime.Now,
                            },
                            new Order()
                            {
                                UserId = 2,
                                OrderNumber = 3,
                                Status = 1,
                                OrderCreateDate = DateTime.Now,
                                OrderUpdateDate = DateTime.Now,
                            }
                        });
                        context.SaveChanges();
                    }

                    if (!context.OrderGoods.Any())
                    {
                        context.OrderGoods.AddRange(new List<OrderGood>()
                        {
                            new OrderGood()
                            {
                                OrderId = 1,
                                GoodsId = 1,
                                Count = 1,
                            },
                            new OrderGood()
                            {
                                OrderId = 1,
                                GoodsId = 2,
                                Count = 2,
                            },
                            new OrderGood()
                            {
                                OrderId = 1,
                                GoodsId = 3,
                                Count = 1,
                            },
                            new OrderGood()
                            {
                                OrderId = 2,
                                GoodsId = 2,
                                Count = 4,
                            },
                            new OrderGood()
                            {
                                OrderId = 3,
                                GoodsId = 2,
                                Count = 1,
                            },
                            new OrderGood()
                            {
                                OrderId = 3,
                                GoodsId = 2,
                                Count = 1,
                            }
                        });
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}

