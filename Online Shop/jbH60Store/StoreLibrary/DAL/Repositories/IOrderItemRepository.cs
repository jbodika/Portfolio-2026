using Microsoft.EntityFrameworkCore;
using StoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreLibrary.DAL.Repositories
{
    public interface IOrderItemRepository
    {


        public DbSet<OrderItem> GetOrderItemContext();

        public void ModifyState(OrderItem orderItem);

        public bool OrderItemExists(int id);

        public void AddOrderItem(OrderItem orderItem);
        public void UpdateOrderItem(OrderItem orderItem);
        public void Save();
        public OrderItem GetOrderItem(int id);

    }
}
