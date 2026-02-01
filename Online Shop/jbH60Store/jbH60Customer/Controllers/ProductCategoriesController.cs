using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using jbH60Customer.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace jbH60Customer.Controllers
{

    [Authorize(Roles ="Customer")]
    public class ProductCategoriesController : Controller
    {
        private readonly HttpClient _httpClient;

        private readonly UserManager<IdentityUser> _userManager;
        public ProductCategoriesController(HttpClient httpClient, UserManager<IdentityUser> userManager)
        {

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5076/api/");
            _userManager = userManager;


        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            string TSShoppingCartTotal = "CartItems/CartItemsCount/" + currentUser.Email;

            ViewBag.CartTotal = await _httpClient.GetFromJsonAsync<int>(TSShoppingCartTotal);
            ViewData["CategoryCounts"] = await _httpClient.GetFromJsonAsync<int>("ProductCategories/AllProductCategoriesCount");


            return View(await _httpClient.GetFromJsonAsync<List<ProductCategory>>("ProductCategories/SortedAlphabetically"));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            var allProducts = await _httpClient.GetFromJsonAsync<IEnumerable<ProductCategory>>("ProductCategories/");
            if (id == null || allProducts == null)
            {
                return NotFound();
            }

            var product = await _httpClient.GetFromJsonAsync<ProductCategory>("ProductCategories/"+id.ToString());

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> Search(string searchName)
        {
            string TSCategoryCount = "Products/CategoryCount";
            var currentUser = await _userManager.GetUserAsync(User);
            string TSShoppingCartTotal = "CartItems/CartItemsCount/" + currentUser.Email;

            ViewBag.CartTotal = await _httpClient.GetFromJsonAsync<int>(TSShoppingCartTotal);

            ViewData["CategoryCounts"] = await _httpClient.GetFromJsonAsync<int>("ProductCategories/AllProductCategoriesCount/"+ searchName.ToString());


            return View("Index", await _httpClient.GetFromJsonAsync<List<ProductCategory>>("ProductCategories/ProductCategorySearch/" + searchName.ToString()));
        }




    }
}
