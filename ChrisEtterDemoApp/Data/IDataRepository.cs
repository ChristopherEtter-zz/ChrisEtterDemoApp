using ChrisEtterDemoApp.Data.EF.Entities;
using System.Collections.Generic;

namespace ChrisEtterDemoApp.Data
{
    public interface IDataRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Order> GetAllOrders(bool includeItems);
        IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems);
        IEnumerable<Product> GetProductsByCategory(string category);
        bool SaveAll();
        Order GetOrderById(string name, int orderId);
        void AddEntity<T>(T model) where T : class;
        void AddOrder(Order newOrder);
    }
}
