using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using jbH60Customer.Models;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Text;
using System.Net;

namespace jbH60Customer.Controllers
{
    [Authorize(Roles = "Customer, Clerk, Manager")]
    public class ShoppingCartsController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly HttpClient _httpClient;

        public ShoppingCartsController(UserManager<IdentityUser> userManager, HttpClient httpClient)
        {

            _userManager = userManager;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5076/api/");
        }

        // GET: ShoppingCarts
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            ViewBag.CartTotal = await _httpClient.GetFromJsonAsync<int>($"CartItems/CartItemsCount/{currentUser.Email}");
            ViewBag.TotalCost = await _httpClient.GetFromJsonAsync<double>($"CartItems/TotalPrice/{currentUser.Email}");
            ViewBag.CartItems = await _httpClient.GetFromJsonAsync<List<CartItem>>($"CartItems/{currentUser.Email}");

            try
            {
                var shoppingCart = await _httpClient.GetFromJsonAsync<ShoppingCart>($"ShoppingCarts/CartsByEmail/{currentUser.Email}");


                if (ViewBag.CartItems.Count == 0)
                {
                    await _httpClient.DeleteAsync($"ShoppingCarts/{shoppingCart.CartId}");

                }
                else if (shoppingCart != null)
                {

                    return View(shoppingCart);
                }
            }
            catch (HttpRequestException)
            {
                return View();

            }

            return View();

        }

        public async Task<IActionResult> ProcessPayment()
        {

            var currentUser = await _userManager.GetUserAsync(User);
            var customer = await _httpClient.GetFromJsonAsync<Customer>($"Customers/CustomerByEmail/{currentUser.Email}");
            var cart = await _httpClient.GetFromJsonAsync<ShoppingCart>($"ShoppingCarts/CartsByEmail/{currentUser.Email}");
            ViewBag.CartItems = await _httpClient.GetFromJsonAsync<List<CartItem>>($"CartItems/{currentUser.Email}");

            ViewBag.Customer = customer;
            ViewBag.TotalCost = await _httpClient.GetFromJsonAsync<double>($"CartItems/TotalPrice/{currentUser.Email}");
            var response = await _httpClient.PostAsJsonAsync($"Orders/CalculateTax/{ViewBag.TotalCost}/{customer.Province}", cart);

            if (response.IsSuccessStatusCode)
            {


                var taxes = Math.Round(Double.Parse(await response.Content.ReadAsStringAsync()),2);

                if(taxes < 0)
                {
                    ViewBag.InvalidProvince = "You cannot but products from this province.";
                    return View();

                }

                ViewBag.Taxes = taxes;
                ViewBag.FinalAmount = (ViewBag.TotalCost +ViewBag.Taxes);
                ViewBag.Customer = customer;

                return View(await _httpClient.GetFromJsonAsync<ShoppingCart>($"ShoppingCarts/CartsByEmail/{currentUser.Email}"));
            }
            else
            {
                return View();
            }
        }


        public async Task<IActionResult> Create()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var cust = await _httpClient.GetFromJsonAsync<Customer>($"Customers/CustomerByEmail/{currentUser.Email}");

            if (cust != null)
            {
                ShoppingCart shoppingCart = new ShoppingCart { CustomerId = cust.CustomerId, DateCreated = DateTime.Now };

                var response = await _httpClient.PostAsJsonAsync("ShoppingCarts", shoppingCart);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

            }

            return RedirectToAction("Error");
        }


        public async Task<IActionResult> AddToCart(CartItem cartItem)
        {

            var currentUser = await _userManager.GetUserAsync(User);
            var existingProductsResponse = await _httpClient.GetAsync($"ShoppingCarts/ExistingProductInShoppingCart/{currentUser.Email}/{cartItem.ProductId}");


            ShoppingCart cart;
            try
            {
                cart = await _httpClient.GetFromJsonAsync<ShoppingCart>($"ShoppingCarts/CartsByEmail/{currentUser.Email}");

            }
            catch (HttpRequestException)
            {

                await Create();
                cart = await _httpClient.GetFromJsonAsync<ShoppingCart>($"ShoppingCarts/CartsByEmail/{currentUser.Email}");

            }
            cartItem.CartId = cart.CartId;

            if (existingProductsResponse.IsSuccessStatusCode)
            {

                var existingCartItem = await existingProductsResponse.Content.ReadFromJsonAsync<CartItem>();

                if (existingCartItem != null)
                {
                    
                    cartItem.CartItemId = existingCartItem.CartItemId;
                    return await UpdateCart(cartItem);
                }
            }



            var response = await _httpClient.PostAsJsonAsync($"CartItems/{currentUser.Email}", cartItem);

            if (response.IsSuccessStatusCode)

            {
                var newCartItem = await response.Content.ReadFromJsonAsync<CartItem>();


                var product = await _httpClient.GetFromJsonAsync<Product>($"CartItems/GetProductByCartItemId/{newCartItem.CartItemId}");
                TempData["NewProduct"] = true;
                TempData["ProductName"] = product.Description;
                TempData["ProductAmount"] = $"({newCartItem.Quantity})";

                return RedirectToAction("Index", "Products");
            }

            TempData["ProductError"] = true;

            return RedirectToAction("Index", "Products", cart);


        }


        public async Task<IActionResult> RemoveFromCart(int id)

        {
            var currentUser = await _userManager.GetUserAsync(User);

            var product = await _httpClient.GetFromJsonAsync<Product>($"CartItems/GetProductByCartItemId/{id}");
            var cartitem = await _httpClient.GetFromJsonAsync<CartItem>($"CartItems/{currentUser.Email}/{id}");
            var response = await _httpClient.DeleteAsync($"CartItems/{id}");

            if (response.IsSuccessStatusCode)
            {

                TempData["RemovedProduct"] = true;
                TempData["ProductName"] = product.Description;
                TempData["ProductAmount"] = $"({cartitem.Quantity})";

                return RedirectToAction("Index", "ShoppingCarts");
            }

            return RedirectToAction("Index", "ShoppingCarts");

        }


        public async Task<IActionResult> UpdateCart(CartItem cartItem)
        {

            var product = await _httpClient.GetFromJsonAsync<Product>($"CartItems/GetProductByCartItemId/{cartItem.CartItemId}");

            var response = await _httpClient.PutAsJsonAsync($"CartItems/{cartItem.CartItemId}", cartItem);


            if (response.IsSuccessStatusCode)
            {
                if (cartItem.Quantity == 0)
                {
                    TempData["RemovedProduct"] = true;
                    TempData["ProductName"] = product.Description;


                }
                else
                {
                    TempData["UpdatedProduct"] = true;
                    TempData["ProductName"] = product.Description;

                    TempData["ProductAmount"] = cartItem.Quantity;
                }



                return RedirectToAction("Index", "ShoppingCarts");
            }

            TempData["ProductError"] = true;
            TempData["ProductName"] = product.Description;

            TempData["ProductAmount"] = $"({cartItem.Quantity})";

            return RedirectToAction("Index", "ShoppingCarts");


        }


        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"ShoppingCarts/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "ShoppingCarts");

            }

            return RedirectToAction("Index", "ShoppingCarts");

        }


    }
}
