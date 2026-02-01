using Microsoft.EntityFrameworkCore;
using StoreLibrary.DAL.Repositories;
using StoreLibrary.Models;
namespace jbH60Services.DAL
{
    public class OrderItemRepository: IOrderItemRepository
    {
        private readonly H60AssignmentDB_jbContext _context;
        public OrderItemRepository(H60AssignmentDB_jbContext context)
        {
            _context = context;
        }

        public DbSet<OrderItem> GetOrderItemContext()
        {
            return _context.OrderItem;
        }

        public void ModifyState(OrderItem orderItem)
        {
            _context.Entry(orderItem).State = EntityState.Modified;
        }
        public bool OrderItemExists(int id)
        {
            return (_context.OrderItem?.Any(e => e.OrderItemId == id)).GetValueOrDefault();
        }


        public void AddOrderItem(OrderItem orderItem)
        {
            _context.OrderItem.Add(orderItem);
        }

        public void UpdateOrderItem(OrderItem orderItem)
        {
            _context.OrderItem.Update(orderItem);
        }

        public OrderItem GetOrderItem(int id)
        {
            return _context.OrderItem.FirstOrDefault(x=>x.OrderItemId == id);
        }


        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
