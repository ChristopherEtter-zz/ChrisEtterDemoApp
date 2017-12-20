using ChrisEtterDemoApp.Data.EF;
using ChrisEtterDemoApp.Data.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChrisEtterDemoApp.Data
{
    public class DataRepository : IDataRepository
    {
        private ChrisEtterDemoAppContext _ctx;
        private ILogger<DataRepository> _logger;

        public DataRepository(ChrisEtterDemoAppContext ctx, ILogger<DataRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public void AddEntity<T>(T model) where T : class
        {
            _ctx.Add(model);
        }

        public void AddOrder(Order newOrder)
        {
            foreach(var item in newOrder.Items)
            {
                item.Product = _ctx.Products.Find(item.Product.Id);
            }
            AddEntity(newOrder);
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            try
            {
                _logger.LogInformation("GetAllOrders was called");
                if(includeItems)
                {
                    return _ctx.Orders
                    .Include(x => x.Items)
                    .ThenInclude(y => y.Product)
                    .OrderBy(x => x.OrderDate)
                    .ToList();
                }
                else
                {
                    return _ctx.Orders
                    .OrderBy(x => x.OrderDate)
                    .ToList();
                }
                
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllOrders error: {ex}");
                return null;
            }
        }

        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems)
        {
            try
            {
                _logger.LogInformation("GetAllOrdersByUser was called");
                if (includeItems)
                {
                    return _ctx.Orders
                        .Where(o => o.User.UserName == username)
                        .Include(x => x.Items)
                        .ThenInclude(y => y.Product)
                        .OrderBy(z => z.OrderDate)
                        .ToList();
                }
                else
                {
                    return _ctx.Orders
                        .Where(o => o.User.UserName == username)
                        .OrderBy(x => x.OrderDate)
                        .ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllOrdersByUser error: {ex}");
                return null;
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("GetAllProducts was called");
                return _ctx.Products
                   .OrderBy(x => x.Category)
                   .ToList();
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetALLProducts error: {ex}");
                return null;
            }
          
        }

        public Order GetOrderById(string name, int orderId)
        {
            try
            {
                _logger.LogInformation("GetAllOrders was called");
                return _ctx.Orders
                    .Include(x => x.Items)
                    .ThenInclude(y => y.Product)
                    .Where(z => z.Id == orderId && z.User.UserName == name)
                    .OrderBy(x => x.OrderDate)
                    .ToList()
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetOrderById error: {ex}");
                return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            _logger.LogInformation("GetProductsByCategory was called");
            return _ctx.Products
               .Where(x => x.Category == category)
               .ToList();
        }

        public bool SaveAll()
        {

            _logger.LogInformation("SaveAll was called");
            return _ctx.SaveChanges() > 0;  
        }

    }
}
