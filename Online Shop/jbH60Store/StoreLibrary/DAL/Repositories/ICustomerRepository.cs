using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StoreLibrary.Models;

namespace StoreLibrary.DAL.Repositories
{
    public interface ICustomerRepository
    {
        DbSet<Customer> GetCustomers();
        Customer GetCustomerById(int id);
        Customer GetCustomerByEmail(string email);

        bool CustomerExists(int id);
        void ModifyState(Customer customer);
        void DeleteCustomer(Customer customer);
        void AddCustomer(Customer customer);

        void UpdateCustomer(Customer customer);
        int GetCustomerOrdersByCustId(int id);
        int GetCustomerShoppingCartsByCustId(int id);
        void Save();
    }
}
