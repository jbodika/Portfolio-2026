using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Threading.Tasks;
using CalculateTaxes;
using CheckCreditCard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreLibrary.DAL.Repositories;
using StoreLibrary.Models;

namespace jbH60Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
     
        private readonly IOrderRepository _orderRepository;

        public OrdersController(H60AssignmentDB_jbContext context, IOrderRepository orderRepository)
        {
            
            _orderRepository = orderRepository;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
          if (_orderRepository.GetOrdersContext() == null)
          {
              return NotFound();
          }
            return await _orderRepository.GetOrdersContext().ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
          if (_orderRepository.GetOrdersContext() == null)
          {
              return NotFound();
          }
            var order = _orderRepository.GetOrderByID(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }


        [HttpGet("LastOrderID")]
        public  ActionResult<int> GetLastOrderID()
        {
            if (_orderRepository.GetOrdersContext() == null)
            {
                return NotFound();
            }

            var order = _orderRepository.GetLastOrderId();

            if (order == 0)
            {
                return NotFound();

            }
            return order;
        }

        // GET: api/Orders/5
        [HttpGet("ByDate/{dateFulfilled}")]
        public async Task<ActionResult<List<Order>>> GetOrdersByDate(DateTime dateFulfilled )
        {
            if (_orderRepository.GetOrdersContext() == null)
            {
                return NotFound();
            }
            var order = _orderRepository.FindByDateFulfilled(dateFulfilled);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }



        // GET: api/Orders/5
        [HttpGet("CustomerOrders/{customerId}")]
        public async Task<ActionResult<List<Order>>> GetAllCustomerOrders(int customerId)
        {
            if (_orderRepository.GetOrdersContext() == null)
            {
                return NotFound();
            }
            var order = _orderRepository.GetAllOrdersByCustomerId(customerId);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _orderRepository.ModifyState(order);

            _orderRepository.Update(order);
            try
            {
                _orderRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_orderRepository.OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
          if (_orderRepository.GetOrdersContext()== null)
          {
              return Problem("Entity set 'H60AssignmentDB_jbContext.Order'  is null.");
          }
            _orderRepository.AddOrder(order);
            _orderRepository.Save();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }


        [HttpPost("CalculateTax/{price}/{province}")]
        public async Task<IActionResult> CalculateTax(decimal price, string province)
        {
            Binding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            EndpointAddress endpoint = new EndpointAddress("https://csdev.cegep-heritage.qc.ca/cartService/calculateTaxes.asmx?op=CalculateTax");

            var client = new CalculateTaxesSoapClient(binding, endpoint);

            var result = await client.CalculateTaxAsync(Decimal.ToDouble(price), province);


            return Ok(result);


        }


        [HttpGet("CreditCard/{cardNum}")]
        public async Task<ActionResult<int>> CreditCardCheck(string cardNum)
        {
            Binding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            EndpointAddress endpoint = new EndpointAddress("https://csdev.cegep-heritage.qc.ca/cartService/checkCreditCard.asmx");

            var client = new CheckCreditCardSoapClient(binding, endpoint);

            var result = await client.CreditCardCheckAsync(cardNum);


            return Ok(result);


        }




        //// DELETE: api/Orders/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOrder(int id)
        //{
        //    if (_context.Order == null)
        //    {
        //        return NotFound();
        //    }
        //    var order = await _context.Order.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Order.Remove(order);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}


    }
}
