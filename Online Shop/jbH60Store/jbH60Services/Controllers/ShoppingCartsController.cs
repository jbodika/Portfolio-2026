using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using CalculateTaxes;
using CheckCreditCard;
using System.Threading.Tasks;
using jbH60Services.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreLibrary.DAL.Repositories;
using StoreLibrary.Models;

namespace jbH60Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
       
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartsController(IShoppingCartRepository shoppingCartRepository)
        {
      
            _shoppingCartRepository = shoppingCartRepository;
        }

        // GET: api/ShoppingCarts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetShoppingCart()
        {
          if (_shoppingCartRepository.GetAllShoppingCarts() == null)
          {
              return NotFound();
          }
            return await _shoppingCartRepository.GetAllShoppingCarts().ToListAsync();
        }

        [HttpGet("CartsByEmail/{email}")]

        public async Task<ActionResult<ShoppingCart>> GetShoppingCartsTest(string email)
        {
            if (_shoppingCartRepository.GetShoppingCartByEmail(email) == null)
            {
                return NotFound(null);
            }
            return  _shoppingCartRepository.GetShoppingCartByEmail(email);
        }


        [HttpGet("ExistingProductInShoppingCart/{email}/{productId}")]
        public ActionResult<CartItem> GetExistingProduct(string email, int productId)
        {
            var existingCartItem = _shoppingCartRepository.GetExistingProduct(email, productId);

            if (existingCartItem == null)
            {
                return NotFound();
            }

            return existingCartItem;
        }
        // GET: api/ShoppingCarts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCart(int id)
        {
            if (_shoppingCartRepository.GetAllShoppingCarts() == null)
            {
                return NotFound();
            }
            var shoppingCart =  _shoppingCartRepository.GetShoppingCartById(id);

            if (shoppingCart == null)
            {
                return NotFound();
            }

            return shoppingCart;
        }

        // PUT: api/ShoppingCarts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShoppingCart(int id, ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.CartId)
            {
                return BadRequest();
            }
            _shoppingCartRepository.ModifyState(shoppingCart);

            try
            {
                 _shoppingCartRepository.SaveShoppingCart();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_shoppingCartRepository.ShoppingCartExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingCart(int id)
        {
            if (_shoppingCartRepository.HasProducts(id))
            {
                return BadRequest("Cannot delete ShoppingCart with products.");

            }
          
            _shoppingCartRepository.DeleteShoppingCart(_shoppingCartRepository.GetShoppingCartById(id));
            _shoppingCartRepository.SaveShoppingCart();
            return Ok();

        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> PostShoppingCart(ShoppingCart shoppingCart)
        {
            if (_shoppingCartRepository.GetAllShoppingCarts() == null)
            {
                return Problem("Entity set 'H60AssignmentDB_jbContext.ShoppingCart'  is null.");
            }
            _shoppingCartRepository.AddShoppingCart(shoppingCart);
            _shoppingCartRepository.SaveShoppingCart();

            return CreatedAtAction("GetShoppingCart", new { id = shoppingCart.CartId }, shoppingCart);
        }

        //[HttpPost("CalculateTax/{price}/{province}")]
        //public async Task<IActionResult> CalculateTax(decimal price,string province)
        //{
        //    Binding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        //    EndpointAddress endpoint = new EndpointAddress("https://csdev.cegep-heritage.qc.ca/cartService/calculateTaxes.asmx?op=CalculateTax");

        //    var client = new CalculateTaxesSoapClient(binding, endpoint);

        //    var result = await client.CalculateTaxAsync(Decimal.ToDouble(price), province);
           
            
        //    return Ok(result);
      

        //}


        //[HttpGet("CreditCard/{cardNum}")]
        //public async Task<IActionResult> CreditCardCheck(string cardNum)
        //{
        //    Binding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        //    EndpointAddress endpoint = new EndpointAddress("https://csdev.cegep-heritage.qc.ca/cartService/checkCreditCard.asmx");

        //    var client = new CheckCreditCardSoapClient(binding, endpoint);

        //    var result = await client.CreditCardCheckAsync(cardNum);


        //    return Ok(result);


        //}



    }
}
