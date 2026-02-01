using Microsoft.EntityFrameworkCore;
using StoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreLibrary.DAL.Repositories
{
    public interface IOrderRepository
    {
        DbSet<Order> GetOrdersContext();
        public void ModifyState(Order order);

        public void Save();
        public void AddOrder(Order order);

        public bool OrderExists(int id);
        public int GetLastOrderId();

        public Order GetOrderByID(int id);
        public void Update(Order order);
        public List<Order> FindByDateFulfilled(DateTime date);
        public List<Order> GetAllOrdersByCustomerId(int id);
    }

}
