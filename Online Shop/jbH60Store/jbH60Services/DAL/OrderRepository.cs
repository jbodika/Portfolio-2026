using Microsoft.EntityFrameworkCore;
using StoreLibrary.DAL.Repositories;
using StoreLibrary.Models;
namespace jbH60Services.DAL
{
    public class OrderRepository : IOrderRepository
    {
        private readonly H60AssignmentDB_jbContext _context;

        public OrderRepository(H60AssignmentDB_jbContext jbContext)
        {
            _context = jbContext;
        }
        public DbSet<Order> GetOrdersContext()
        {
            return _context.Order;
        }

        public void ModifyState(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
        }
        public void  Save()
        {

             _context.SaveChangesAsync();
        }
        
        public List<Order> GetAllOrdersByCustomerId(int id) { 
        
            return _context.Order.Include(x => x.Customer).Where(x=>x.CustomerId ==id).ToList();
        }

        public int GetLastOrderId()
        {
            var lastOrder = _context.Order.OrderByDescending(x => x.OrderId).First();

            if (lastOrder != null)
            {
                return lastOrder.OrderId;
            }

            return 0;

        }


        public List<Order> FindByDateFulfilled(DateTime date)
        {
            return _context.Order.Where(x=>x.DateFufilled==date).ToList();
        }
        public void Update(Order order)
        {

            _context.Order.Update(order);
        }

        public void AddOrder(Order order)
        {
            _context.Order.Add(order);
        }

        public bool OrderExists(int id)
        {
            return (_context.Order?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }

        public  Order GetOrderByID(int id)
        {
            //return  _context.Order.Find(id);
             return _context.Order.First(m => m.OrderId == id);
        }
    }
}
