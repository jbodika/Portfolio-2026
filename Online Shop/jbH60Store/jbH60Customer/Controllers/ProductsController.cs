using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using jbH60Customer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;

namespace jbH60Customer.Controllers
{
    [Authorize(Roles = "Customer, Clerk, Manager")]
    public class ProductsController : Controller
    {
       
        private readonly HttpClient _httpClient;

        private readonly UserManager<IdentityUser> _userManager;
        public ProductsController( HttpClient httpClient, UserManager<IdentityUser> userManager)
        {
          
            _httpClient = httpClient;
            _userManager = userManager;
            _httpClient.BaseAddress = new Uri("http://localhost:5076/api/");

        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            ShoppingCartTotal();
            string TSCategoryCount = "Products/CategoryCount";
            string TSProdCount = "Products/ProductCount";


            ViewData["CategoryCounts"] = await _httpClient.GetFromJsonAsync<Dictionary<string, int>>(TSCategoryCount);
            ViewData["ProdCount"] = await _httpClient.GetFromJsonAsync<int>(TSProdCount);
       
            return View(await _httpClient.GetFromJsonAsync<IEnumerable<Product>>("Products/SortDescAlphabetically"));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ShoppingCartTotal();
            var allProducts = await _httpClient.GetFromJsonAsync<IEnumerable<Product>>("Products");
            if (id == null || allProducts == null)
            {
                return NotFound();
            }

            var product = await _httpClient.GetFromJsonAsync<Product>(id.ToString());

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task <IActionResult> Search(string searchName)
        {
            ShoppingCartTotal();
            string TSCategoryCount = "Products/CategoryCount";
            string TSProdCount = "Products/SearchResults/" + searchName;


            ViewData["CategoryCounts"] = await _httpClient.GetFromJsonAsync<Dictionary<string, int>>(TSCategoryCount);
            ViewData["ProdCount"] = await _httpClient.GetFromJsonAsync<int>(TSProdCount);
            return View("Index",await _httpClient.GetFromJsonAsync<List<Product>>("Products/ProductSearch/" + searchName.ToString()));
        }


        public async Task <IActionResult> ViewByProdCat(int id)
        {
            ShoppingCartTotal();
            string TSProductCat = "Products/AllProductsForProductCategory/" + id.ToString();
            string TSCategoryCount = "Products/CategoryCount";


            ViewData["CategoryCounts"] = await _httpClient.GetFromJsonAsync<Dictionary<string, int>>(TSCategoryCount);
            ViewData["ProdCount"] = await _httpClient.GetFromJsonAsync<int>($"Products/ProductCountForProductCategory/{id}");

            return View(await _httpClient.GetFromJsonAsync<IEnumerable<Product>>(TSProductCat));
        }
    private async void ShoppingCartTotal()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        string TSShoppingCartTotal = "CartItems/CartItemsCount/" + currentUser.Email;

        ViewBag.CartTotal = await _httpClient.GetFromJsonAsync<int>(TSShoppingCartTotal);

    }


    }


}
