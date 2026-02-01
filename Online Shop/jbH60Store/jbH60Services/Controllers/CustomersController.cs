using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jbH60Services.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreLibrary.DAL;
using StoreLibrary.DAL.Repositories;
using StoreLibrary.Models;



namespace jbH60Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
   
        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            if (_customerRepository.GetCustomers() == null)
            {
                return NotFound();
            }
            return Ok(_customerRepository.GetCustomers());
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            if (_customerRepository.GetCustomers() == null)
            {
                return NotFound();
            }

            var customer = _customerRepository.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok( customer);
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

           _customerRepository.ModifyState(customer);

            try
            {
                _customerRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_customerRepository.CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpGet("CustomerByEmail/{email}")]
        public async Task<ActionResult<Customer>> GetCustomerByEmail(string email)
        {
            if (_customerRepository.GetCustomerByEmail(email) == null)
            {
                return NotFound();
            }
            return Ok(_customerRepository.GetCustomerByEmail(email));
        }


        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {

      

            if (_customerRepository.GetCustomers() == null)
            {
                return Problem("Entity set 'H60AssignmentDB_jbContext.Customers'  is null.");
            }
            _customerRepository.AddCustomer(customer);
            _customerRepository.Save();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (_customerRepository.GetCustomers() == null)
            {
                return NotFound();
            }
            var customer = _customerRepository.GetCustomerById(id);


            if (customer == null)
            {
                return NotFound();

            }else if(_customerRepository.GetCustomerShoppingCartsByCustId(id) ==0 && _customerRepository.GetCustomerOrdersByCustId(id) == 0)
            {
                _customerRepository.DeleteCustomer(customer);
                _customerRepository.Save();
                 return NoContent();
            }

           
            return NotFound();
           
        }

        [HttpGet("CustOrders/{id}")]
        public async Task <int> GetOrders(int id)
        {
            return _customerRepository.GetCustomerOrdersByCustId(id);
        }
        [HttpGet("CustShoppingCarts/{id}")]
        public async Task<int> GetCarts(int id)
        {
            return _customerRepository.GetCustomerShoppingCartsByCustId(id);
        }

  


    }
}
