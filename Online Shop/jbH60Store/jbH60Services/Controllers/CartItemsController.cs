using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jbH60Services.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreLibrary.DAL.Repositories;
using StoreLibrary.Models;
using StoreLibrary.DTO;

namespace jbH60Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemRepository _cartItemRepository;

        public CartItemsController( ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        // GET: api/CartItems
        [HttpGet("{email}")]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetCartItems(string email)
        {
            if (_cartItemRepository.GetAllCartItems(email) == null)
            {
                return NotFound();
            }
            return  _cartItemRepository.GetAllCartItems(email);
        }

        // GET: api/CartItems/5
        [HttpGet("{email}/{id}")]
        public async Task<ActionResult<CartItem>> GetCartItem(string email, int id)
        {
            if (_cartItemRepository.GetAllCartItems(email) == null)
            {
                return NotFound();
            }
            var cartItem =  _cartItemRepository.GetCartItemById(id);

            if (cartItem == null)
            {
                return NotFound();
            }

            return cartItem;
        }

        [HttpGet("GetProductByCartItemId/{id}")]

        public async Task<ActionResult<Product>> GetProduct(int id)

        {
            if (id ==0)
            {
                return NotFound("Invalid request. ");
            }
           
            return _cartItemRepository.GetProductByCartItemId(id);
        }

            // PUT: api/CartItems/5
            [HttpPut("{id}")]
        public async Task<IActionResult> PutCartItem(int id, CartItem cartItem)
        {
            if (id != cartItem.CartItemId)
            {
                return BadRequest("Invalid request. The provided ID does not match the ID in the request body.");
            }

           
            var existingCartItem = _cartItemRepository.GetCartItemById(id);

            if (existingCartItem == null)
            {
                return NotFound($"CartItem with ID {id} not found");
            }

            // Get the associated product based on the cartItem.ProductId
            var product = _cartItemRepository.GetProduct(cartItem.ProductId);

            if (product == null)
            {
                return NotFound($"Product with ID {cartItem.ProductId} not found");
            }

            _cartItemRepository.UpdatesOnQuantity(existingCartItem, cartItem);

            try
            {
                _cartItemRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_cartItemRepository.CartItemExists(id))
                {
                    return NotFound($"CartItem with ID {id} not found after concurrency update.");
                }
                else
                {
                    throw;
                }
            }
          //  return CreatedAtAction("GetCartItem", new { id = cartItem.CartItemId }, cartItem);
            return Ok();    
        }



        [HttpPost("{email}")]
        public async Task<ActionResult<CartItem>> PostCartItem(string email, [FromBody] CartItem model)
        {
            if (model == null)
            {
                return BadRequest("Invalid request body");
            }

            // Get the associated product based on the cartItem.ProductId
            var product =  _cartItemRepository.GetProduct(model.ProductId);

            if (product == null)
            {
                return NotFound($"Product with ID {model.ProductId} not found");
            }


            // Modify the product stock based on the quantity of the new cart item
            product.Stock -= model.Quantity;

             _cartItemRepository.AddCartItem(model);
             _cartItemRepository.Save();

            return CreatedAtAction("GetCartItem", new { id = model.CartItemId, email = email }, model);
           
            //return Ok();
        }


        [HttpGet("CartItemsCount/{email}")]
        public async Task<ActionResult<int>> GetCartItemsCount(string email)
        {
            if (_cartItemRepository.GetAllCartItems(email) == null)
            {
                return NotFound();
            }
            var cartItem = _cartItemRepository.GetCartItemsCount(email);

            if (cartItem == 0)
            {
                return 0;
            }

            return cartItem;
        }


        [HttpGet("TotalPrice/{email}")]
        public async Task<ActionResult<decimal>> GetTotalCost(string email)
        {
            if (_cartItemRepository.GetTotalPrice(email) == 0)
            {
                return 0;
            }
            var cartItem = _cartItemRepository.GetTotalPrice(email);

            if (cartItem == 0)
            {
                return 0;
            }

            return Ok(cartItem);

        

        }


        // DELETE: api/CartItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            if (id == null)
            {
                return BadRequest("Invalid request body");
            }

          
            var cartItem =  _cartItemRepository.GetCartItemById(id);

            if (cartItem == null)
            {
                return NotFound($"CartItem with Id {id} not found");
            }
            var product = _cartItemRepository.GetProduct(cartItem.ProductId);

            if (product == null)
            {
                return NotFound($"Product with ID {cartItem.ProductId} not found");
            }

            // Update the product stock based on the quantity of the existing cart item
            product.Stock += cartItem.Quantity;

            _cartItemRepository.DeleteCartItem(cartItem);
            _cartItemRepository.Save();

            return Ok("Cart Item deleted");
        }



        // DELETE: api/CartItems/5
        [HttpDelete("OrderCompletion/{id}")]
        public async Task<IActionResult> DeleteCartItemForOrderCompletion(int id)
        {
            if (id == null)
            {
                return BadRequest("Invalid request body");
            }


            var cartItem = _cartItemRepository.GetCartItemById(id);

            if (cartItem == null)
            {
                return NotFound($"CartItem with Id {id} not found");
            }
            var product = _cartItemRepository.GetProduct(cartItem.ProductId);

            if (product == null)
            {
                return NotFound($"Product with ID {cartItem.ProductId} not found");
            }

            // Update the product stock based on the quantity of the existing cart item
          

            _cartItemRepository.DeleteCartItem(cartItem);
            _cartItemRepository.Save();

            return Ok("Cart Item deleted");
        }


    }
}
