using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using jbH60Store.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Net;

namespace jbH60Store.Controllers
{
    [Authorize(Roles = "Manager, Clerk")]
    public class ProductCategoriesController : Controller
    {
    
        private readonly HttpClient _httpClient;
        public ProductCategoriesController(HttpClient httpClient)
        {
          _httpClient = httpClient;
          _httpClient.BaseAddress = new Uri("http://localhost:5076/api/ProductCategories/");
        }




    
        public async Task<IActionResult> Details(int id)
        {
            string TSProdCat = id.ToString();
            if (id == null)
            {
                return NotFound();
            }

            var productCat = await _httpClient.GetFromJsonAsync<ProductCategory>(TSProdCat);

            if (productCat == null)
            {
                return NotFound();
            }

            return View(productCat);
        }



        public IActionResult Create()
        {
            return View();
        }

    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCategory productCategory)
        {
            

            var response = await _httpClient.PostAsJsonAsync("", productCategory);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("AllProductCategoriesSortedByCategory");
            }

            return View(productCategory);
        }


        public async Task <IActionResult> AllProductCategoriesSortedByCategory()
        {
            string TSProdCats = "SortedAlphabetically";
            
            return View("AllProducts", await _httpClient.GetFromJsonAsync<IEnumerable<ProductCategory>>(TSProdCats));
        }

        public async Task<IActionResult> Edit(int id)
        {

            return await Details(id);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,ProdCat")] ProductCategory productCategory)
        {
            string TSProdCat = $"{id}";

            if (id == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  
                    var response = await _httpClient.PutAsJsonAsync(TSProdCat, productCategory);

                    if (response.IsSuccessStatusCode)
                    {
                      return RedirectToAction("AllProductInformation");
                    }
                    else
                    {
                    
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (productCategory == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

        

            return View("Details", productCategory);
        }


        public async Task<IActionResult> Delete(int id)
        {
            string TSProdCat = id.ToString();
            var productCategory = await _httpClient.GetFromJsonAsync<ProductCategory>(TSProdCat);
            ViewData["ProductCount"] = await _httpClient.GetFromJsonAsync<int>($"Count/{id}");

            if (id == null || productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);

        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
      
                string TSProdCat = id.ToString();
                var productCategory = await _httpClient.GetFromJsonAsync<ProductCategory>(TSProdCat);
         

                if (id == null || productCategory == null)
                {
                    return NotFound();
                }

                var response = await _httpClient.DeleteAsync(TSProdCat);

                if (response.IsSuccessStatusCode)
                {

                   return RedirectToAction("AllProductCategoriesSortedByCategory"); 

                }
                else
                {
                 
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        TempData["ErrorMessage"] = "Product category not found.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "An error occurred while deleting the product category.";
                    }

                    return RedirectToAction("Error"); 
                }

     

        }


    }
}
