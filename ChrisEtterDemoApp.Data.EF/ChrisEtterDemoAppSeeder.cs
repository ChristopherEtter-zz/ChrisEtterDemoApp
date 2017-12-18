using ChrisEtterDemoApp.Data.EF.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChrisEtterDemoApp.Data.EF
{
    public class ChrisEtterDemoAppSeeder
    {
        private ChrisEtterDemoAppContext _ctx;
        private IHostingEnvironment _hosting;
        private UserManager<StoreUser> _userManager;

        public ChrisEtterDemoAppSeeder(ChrisEtterDemoAppContext ctx, IHostingEnvironment hosting, UserManager<StoreUser> userManager)
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async Task Seed()
        {
            _ctx.Database.EnsureCreated();

            var user = await _userManager.FindByEmailAsync("christopheretter@outlook.com");

            if(user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Chris",
                    LastName = "Etter",
                    UserName = "christopheretter@outlook.com",
                    Email = "christopheretter@outlook.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd");
                if(result == IdentityResult.Success)
                {
                    throw new InvalidOperationException("Failed to create Default User");
                }
            }

            if(!_ctx.Products.Any())
            {
                //Need to create sample data
                var filePath = Path.Combine(_hosting.ContentRootPath,"Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                _ctx.Products.AddRange(products);

                var order = new Order()
                {
                    OrderDate = DateTime.Now,
                    OrderNumber = "12345",
                    User = user,
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    }
                };

                _ctx.Orders.Add(order);

                _ctx.SaveChanges();


            }
        }
    }
}
