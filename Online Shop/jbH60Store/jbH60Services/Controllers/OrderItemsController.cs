using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreLibrary.DAL.Repositories;
using StoreLibrary.Models;

namespace jbH60Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemsController( IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItem()
        {
          if (_orderItemRepository.GetOrderItemContext() == null)
          {
              return NotFound();
          }
            return await _orderItemRepository.GetOrderItemContext().ToListAsync();
        }

        // GET: api/OrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
          if (_orderItemRepository.GetOrderItemContext() == null)
          {
              return NotFound();
          }
            var orderItem =_orderItemRepository.GetOrderItem(id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return orderItem;
        }

        // PUT: api/OrderItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(int id, OrderItem orderItem)
        {
            if (id != orderItem.OrderItemId)
            {
                return BadRequest();
            }

             _orderItemRepository.ModifyState(orderItem);

            try
            {
                _orderItemRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_orderItemRepository.OrderItemExists(id))
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

        // POST: api/OrderItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItem orderItem)
        {
          if (_orderItemRepository.GetOrderItemContext() == null)
          {
              return Problem("Entity set 'H60AssignmentDB_jbContext.OrderItem'  is null.");
          }
            _orderItemRepository.AddOrderItem(orderItem);
            _orderItemRepository.Save();

            return CreatedAtAction("GetOrderItem", new { id = orderItem.OrderItemId }, orderItem);
        }

        //// DELETE: api/OrderItems/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOrderItem(int id)
        //{
        //    if (_context.OrderItem == null)
        //    {
        //        return NotFound();
        //    }
        //    var orderItem = await _context.OrderItem.FindAsync(id);
        //    if (orderItem == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.OrderItem.Remove(orderItem);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

   
    }
}
