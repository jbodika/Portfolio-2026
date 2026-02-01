using Microsoft.EntityFrameworkCore;
using StoreLibrary.DAL.Repositories;
using StoreLibrary.Models;
using StoreLibrary.DTO;

namespace jbH60Services.DAL
{
    public class CustomerRepository: ICustomerRepository
    {
        private readonly H60AssignmentDB_jbContext _context;

        public CustomerRepository(H60AssignmentDB_jbContext context)
        {
            _context = context;
        }

        public DbSet<Customer> GetCustomers()
        {
            return _context.Customer;
        }

       public Customer GetCustomerById(int id)
        {
            return _context.Customer.Where(x => x.CustomerId == id).First();
        }
        public void ModifyState(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
        }

        public bool CustomerExists(int id)
        {
            return (_context.Customer?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }

        public void DeleteCustomer(Customer customer)
        {
            _context.Customer.Remove(customer);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void AddCustomer(Customer customer)
        {
           _context.Customer.Add(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
           _context.Customer.Update(customer);
        }

        public int GetCustomerOrdersByCustId(int id)
        {
         return   _context.Order.Where(x => x.CustomerId == id).Count();
        }

        public int GetCustomerShoppingCartsByCustId(int id)
        {
            return _context.ShoppingCart.Where(x => x.CustomerId == id).Count();

        }

        public Customer GetCustomerByEmail(string email)
        {

            return _context.Customer.Where(x => x.Email == email).FirstOrDefault();
        }
    }
}
