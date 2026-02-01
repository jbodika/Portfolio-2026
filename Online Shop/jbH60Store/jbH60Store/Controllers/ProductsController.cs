using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using jbH60Store.Models;

using System.Net;

using System.Text;


namespace jbH60Store.Controllers
{
    [Authorize(Roles = "Manager, Clerk")]
    public class ProductsController : Controller
    {
        private readonly HttpClient _httpClient;
        public ProductsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5076/api/");

        }


        public async Task<IActionResult> AllProductInformation()
        {

            string TaskString = "Products/SortDescAlphabetically";
            string TSCategoryCount = "Products/CategoryCount";
            string TSProdCount = "Products/ProductCount";


            ViewData["CategoryCounts"] = await _httpClient.GetFromJsonAsync<Dictionary<string, int>>(TSCategoryCount);
            ViewData["ProdCount"] = await _httpClient.GetFromJsonAsync<int>(TSProdCount);
            return View("ShortenedInformation", await _httpClient.GetFromJsonAsync<IEnumerable<Product>>(TaskString));

        }




        public async Task<IActionResult> SortByProductCategory(Product product)
        {

            string TaskString = "Products/SortProdCatAlphabetically";
            string TSCategoryCount = "Products/CategoryCount";


            ViewData["CategoryCounts"] = await _httpClient.GetFromJsonAsync<Dictionary<string, int>>(TSCategoryCount);

            return View("ViewByProductCategory", await _httpClient.GetFromJsonAsync<IEnumerable<Product>>(TaskString));
        }

        public async Task<IActionResult> Edit(int? id)
        {

            string TSGetProduct = "Products/" + id.ToString();
            string TSProdCats = "ProductCategories";
       
            if (id == null)
            {
                return NotFound();
            }
            var product = await _httpClient.GetFromJsonAsync<Product>(TSGetProduct);

            if (product == null)
            {
                return NotFound();
            }

            ViewData["ProdCatId"] = new SelectList(await _httpClient.GetFromJsonAsync<IEnumerable<ProductCategory>>(TSProdCats), "CategoryId", "ProdCat", product.ProdCatId);

            return View(product);

        }
        public async Task<IActionResult> ViewByProductCategory(int id)
        {
            string TSProductCat = "Products/AllProductsForProductCategory/" + id.ToString();
            string TSCategoryCount = "Products/CategoryCount";

            ViewData["CategoryCounts"] = await _httpClient.GetFromJsonAsync<Dictionary<string, int>>(TSCategoryCount);


            return View(await _httpClient.GetFromJsonAsync<IEnumerable<Product>>(TSProductCat));
        }

        public async Task<IActionResult> Search(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                TempData["ErrorMessage"] = "Please enter a product name to search";
                return RedirectToAction("PageNotFound", "Error");
            }

            string TSSearch = "Products/ProductSearch/" + productName.ToString();
            string TSSearchCount = "Products/SearchResults/" + productName.ToString();
            string TSCategoryCount = "Products/CategoryCount";
            TempData["SearchVal"] = productName;

            ViewData["CategoryCounts"] = await _httpClient.GetFromJsonAsync<Dictionary<string, int>>(TSCategoryCount);

            int productCount = await _httpClient.GetFromJsonAsync<int>(TSSearchCount);

            if (productCount == 0)
            {
                TempData["ErrorMessage"] = "Could not find any products that contain '" + productName + "'";
                return RedirectToAction("PageNotFound", "Error");
            }
            else
            {
                ViewData["ProdCount"] = productCount;
                return View("ShortenedInformation", await _httpClient.GetFromJsonAsync<List<Product>>(TSSearch));
            }



        }

        public async Task<IActionResult> Details(int? id)
        {
            string TSProduct = "Products/" + id.ToString();

            var product = await _httpClient.GetFromJsonAsync<Product>(TSProduct);
            if (id == null || product == null)
            {
                return NotFound();
            }


            return View(product);
        }



        public async Task<IActionResult> Delete(int? id)
        {
            string TSProduct = "Products/" + id.ToString();

            var product = await _httpClient.GetFromJsonAsync<Product>(TSProduct);
            if (id == null || product == null)
            {
                return NotFound();
            }


            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string TSDeleteProduct = $"Products/{id}";

            var response = await _httpClient.DeleteAsync(TSDeleteProduct);

            if (response.IsSuccessStatusCode)
            {
                return await AllProductInformation();
            }
            else
            {
          
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["ErrorMessage"] = "Product not found.";
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while deleting the product.";
                }

                return RedirectToAction("Error"); 
            }
        }

        public async Task<IActionResult> Create()
        {
            string TSProdCats = "ProductCategories";

            ViewData["ProdCatId"] = new SelectList(await _httpClient.GetFromJsonAsync<IEnumerable<ProductCategory>>(TSProdCats), "CategoryId", "ProdCat");


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProdCatId,Description,Manufacturer,Stock,BuyPrice,SellPrice,EmployeeNotes,Image")] Product product, IFormFile? imageFile)
        {
            string TSProduct = "Products/";
            string TSProdCats = "ProductCategories";

            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(memoryStream);
                        product.Image = memoryStream.ToArray();
                    }
                }


                var response = await _httpClient.PostAsJsonAsync(TSProduct, product);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AllProductInformation");
                }
            }

            ViewData["ProdCatId"] = new SelectList(await _httpClient.GetFromJsonAsync<IEnumerable<ProductCategory>>(TSProdCats), "CategoryId", "ProdCat", product.ProdCatId);

            return View(product);
        }


        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdatePrices(int? id, Product product)
        {

            string TSProduct = $"Products/{id}";
            string TSProdCats = "ProductCategories";

            if (id == null)
            {
                return NotFound();
            }
            var priceToUpdate = await _httpClient.GetFromJsonAsync<Product>(TSProduct);

            if (product == null)
            {
                return NotFound();
            }

            ViewData["ProdCatId"] = new SelectList(await _httpClient.GetFromJsonAsync<IEnumerable<ProductCategory>>(TSProdCats), "CategoryId", "ProdCat", product.ProdCatId);

            return View(priceToUpdate);
        }


        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePrices(int id, string newBuyPrice, string newSellPrice)
        {
            string TSStock = $"Products/UpdatePrices/{id}";
            string TSProduct = $"Products/{id}";

            var product = await _httpClient.GetFromJsonAsync<Product>(TSProduct);
            string TSProdCats = "ProductCategories";
            ViewData["ProdCatId"] = new SelectList(await _httpClient.GetFromJsonAsync<IEnumerable<ProductCategory>>(TSProdCats), "CategoryId", "ProdCat", product.ProdCatId);

            decimal buyPrice, sellPrice;

            if (!decimal.TryParse(newBuyPrice, out buyPrice) || !decimal.TryParse(newSellPrice, out sellPrice))
            {
              
                ModelState.AddModelError("Prices", "Buy price and sell price must be valid numbers.");
                return View("Edit", product);
            }

            if (buyPrice < 0 || sellPrice < 0)
            {
              
                ModelState.AddModelError("Prices", "Prices cannot be negative.");
                return View("Edit", product);
            }

            if (buyPrice == 0 || sellPrice == 0)
            {
              
                ModelState.AddModelError("Prices", "Prices cannot be equal to 0.");
                return View("Edit", product);
            }

            if (sellPrice < buyPrice)
            {
              
                ModelState.AddModelError("Prices", "Sell price must be greater than or equal to buy price.");
                return View("Edit", product);
            }

          
            var updateData = new
            {
                BuyPrice = newBuyPrice,
                SellPrice = newSellPrice
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json");

            var response = await _httpClient.PatchAsync(TSStock, jsonContent);

            if (response.IsSuccessStatusCode)
            {
              
                product = await _httpClient.GetFromJsonAsync<Product>(TSProduct);
             
            }
         

         
            return RedirectToAction("Edit", new { id = product.ProductId });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStock(int id, int? newStock)
        {
            string TSStock = $"Products/UpdateStock/{id}";  
            string TSProduct = $"Products/{id}";
            string TSProdCats = "ProductCategories";

            if (id == null)
            {
                return NotFound();
            }

            var product = await _httpClient.GetFromJsonAsync<Product>(TSProduct);

            ViewData["ProdCatId"] = new SelectList(await _httpClient.GetFromJsonAsync<IEnumerable<ProductCategory>>(TSProdCats), "CategoryId", "ProdCat", product.ProdCatId);

            if (product == null)
            {
                return NotFound();
            }

            if (newStock == null)
            {
                ModelState.AddModelError("NewStock", "New stock value is required.");
                return View("Edit", product);
            }

            int updatedStock;

            if (newStock < 0)
            {
                updatedStock = product.Stock - Math.Abs(newStock.Value);
            }
            else
            {
                updatedStock = product.Stock + newStock.Value;
            }

            if (updatedStock < 0)
            {
                ModelState.AddModelError("NewStock", "Stock cannot go below 0.");
                return View("Edit", product);
            }

        
            var updateData = new
            {
                NewStock = updatedStock
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(TSStock, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Edit", new { id = product.ProductId });
            }
            else
            {
              
                ModelState.AddModelError("UpdateFailed", "Failed to update stock.");
                return View("Edit", product);
            }
        }
        //GET
        public async Task<IActionResult> UpdateStock(int? id, Product product)
        {

            string TSProduct = $"Products/{id}";
            string TSProdCats = "ProductCategories";


            if (id == null)
            {
                return NotFound();
            }


            var stockToUpdate = await _httpClient.GetFromJsonAsync<Product>(TSProduct);

            if (product == null)
            {
                return NotFound();
            }

            ViewData["ProdCatId"] = new SelectList(await _httpClient.GetFromJsonAsync<IEnumerable<ProductCategory>>(TSProdCats), "CategoryId", "ProdCat", product.ProdCatId);


            return View(stockToUpdate);
        }






        public async Task<IActionResult> RemoveImage(int id)
        {
            var response = await _httpClient.DeleteAsync($"Products/ProductImage/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Handle errors based on the response status code
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["ErrorMessage"] = "Image not found.";
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while deleting the product.";
                }

                return RedirectToAction("Error"); // Redirect to an error page or action

            }
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProdCatId,Description,Manufacturer,Stock,BuyPrice,SellPrice,EmployeeNotes,Image")] Product product, IFormFile? imageFile)
        {
            string TSProduct = $"Products/{id}";

            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // If a new image file is uploaded, update the product's image
                        using (var memoryStream = new MemoryStream())
                        {
                            await imageFile.CopyToAsync(memoryStream);
                            product.Image = memoryStream.ToArray();
                        }
                    }
                    else
                    {
         
                        var existingProduct =await _httpClient.GetFromJsonAsync<Product>(TSProduct);
                        if (existingProduct != null)
                        {
                            product.Image = existingProduct.Image;
                        }
                    }

                
                    var response = await _httpClient.PutAsJsonAsync(TSProduct, product);

                    if (response.IsSuccessStatusCode)
                    {
                  
                    }
                    else
                    {
                       
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (product == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // If there are validation errors, return to the view with the current product data.
            string TSProdCats = "ProductCategories";
            ViewData["ProdCatId"] = new SelectList(await _httpClient.GetFromJsonAsync<IEnumerable<ProductCategory>>(TSProdCats), "CategoryId", "ProdCat", product.ProdCatId);

            return View("Details", product);
        }



    }
}




