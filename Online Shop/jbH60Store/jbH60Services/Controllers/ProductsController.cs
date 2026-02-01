using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using jbH60Services.DAL;
using StoreLibrary.DAL.Repositories;
using StoreLibrary.Models;
using StoreLibrary.DTO;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

namespace jbH60Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductRepository _productRepository;
        public ProductsController(IProductRepository productRepository)
        {

            _productRepository = productRepository;


        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            if (_productRepository.GetProductContext() == null)
            {
                return NotFound();
            }
            return Ok(_productRepository.GetProductContext());
        }


        // GET: api/Products
        [HttpGet("SearchProductByStock/{num}")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProductByStock(int num)
        {
            if (_productRepository.GetProductContext() == null)
            {
                return NotFound();
            }
            return Ok(_productRepository.SearchByStock(num));
        }        // GET: api/Products
        [HttpGet("SearchProductBySellPrice/{price}")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProductBySellPrice(decimal price)
        {
            if (_productRepository.GetProductContext() == null)
            {
                return NotFound();
            }
            return Ok(_productRepository.SearchBySellPrice(price));
        }        // GET: api/Products
        [HttpGet("SearchProductByBuyPrice/{price}")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProductByBuyPrice(decimal price)
        {
            if (_productRepository.GetProductContext() == null)
            {
                return NotFound();
            }
            return Ok(_productRepository.SearchByBuyPrice(price));
        }



        [HttpDelete("ProductImage/{id}")]
        public async Task<ActionResult<Product>> DeleteProductImage(int id)
        {

            if (_productRepository.GetProductContext() == null)
            {
                return Problem("Entity set 'H60AssignmentDB_jbContext.Products'  is null.");
            }


            var product = _productRepository.GetProductById(id);

            _productRepository.DeleteProductImage(id);


            _productRepository.ModifyState(product);

            try
            {
                _productRepository.Save();
                return Ok();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_productRepository.ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }




        }



        // GET: api/Products
        [HttpGet("SortDescAlphabetically")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsSortedByDesc()
        {

            try
            {
                var sortedProducts = _productRepository.SortByDescription();

                if (sortedProducts == null || !sortedProducts.Any())
                {
                    return NotFound("No sorted products found.");
                }

                return Ok(sortedProducts);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while fetching sorted products.");
            }
        }


        [HttpGet("SortProdCatAlphabetically")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsSortedByProdCat()
        {

            try
            {
                var sortedProducts = _productRepository.SortByProdCat();

                if (sortedProducts == null || !sortedProducts.Any())
                {
                    return NotFound("No sorted products found.");
                }

                return Ok(sortedProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching sorted products.");
            }
        }





        [HttpGet("CategoryCount")]
        public async Task<ActionResult<Dictionary<string, int>>> GetCategoryCount()
        {
            if (_productRepository.GetCategoryCount() == null || !_productRepository.GetCategoryCount().Any())
            {
                return NotFound("Nothing to see here!");
            }
            return Ok(_productRepository.GetCategoryCount());
        }
        [HttpGet("ProductCount")]
        public async Task<ActionResult<int>> GetProductCount()
        {

            if (_productRepository.GetProductCount() == 0)
            {
                return NotFound("There's nothing");
            }
            return Ok(_productRepository.GetProductCount());
        }



        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {


            if (_productRepository.GetProductContext() == null)
            {
                return NotFound();
            }
            var product = _productRepository.GetProductByIdDTO(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        [HttpGet("ProductCategories")]

        public async Task<DbSet<ProductCategory>> GetProductCategories()
        {
            return _productRepository.GetProductCategories();
        }

        //// PUT: api/Products/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _productRepository.ModifyState(product);

            try
            {
                _productRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_productRepository.ProductExists(id))
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


        [HttpGet("ProductSearch/{productName}")]
        public async Task<ActionResult<List<ProductDTO>>> ProductSearch(string productName)
        {
            var product = _productRepository.PartialProductSearch(productName);

            if (product == null)
            {
                return NotFound(productName);

            }
            return Ok(product);
        }


        [HttpGet("AllProductsForProductCategory/{id}")]
        public async Task<ActionResult<List<Product>>> AllProductsForProductCategory(int id)
        {
            var prodCat = _productRepository.AllProductsForProductCategory(id);

            if (prodCat == null)
            {
                return NotFound();
            }
            return Ok(prodCat);
        }
        [HttpGet("ProductCountForProductCategory/{id}")]
        public async Task<ActionResult<List<Product>>> ProductCountForProductCategory(int id)
        {
            var prodCat = _productRepository.ProductCountForProductCategory(id);

            if (prodCat == null)
            {
                return NotFound();
            }
            return Ok(prodCat);
        }



        [HttpGet("SearchResults/{productName}")]
        public async Task<ActionResult<int>> SearchResults(string productName)
        {
            var searchCount = _productRepository.SearchResultsCount(productName);

            if (searchCount == null)
            {
                return NotFound();
            }
            return Ok(searchCount);
        }

        [HttpPatch("UpdateStock/{id}")]
        public async Task<ActionResult<Product>> UpdateProductStock(int id, [FromBody] ProductStockDTO stockUpdate)
        {
            var product = _productRepository.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            // Update the stock of the product with stockUpdate request.
            product.Stock = stockUpdate.NewStock;

            _productRepository.ModifyState(product);

            try
            {
                _productRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_productRepository.ProductExists(id))
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

        [HttpPatch("UpdatePrices/{id}")]
        public async Task<ActionResult<Product>> UpdatePrices(int id, [FromBody] ProductPricesDTO product)
        {
            var existingProduct = _productRepository.GetProductById(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.BuyPrice = product.BuyPrice;
            existingProduct.SellPrice = product.SellPrice;
            _productRepository.ModifyState(existingProduct);
            try
            {

                _productRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_productRepository.ProductExists(id))
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



        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            if (_productRepository.GetProductContext() == null)
            {
                return Problem("Entity set 'H60AssignmentDB_jbContext.Products'  is null.");
            }
            _productRepository.AddProduct(product);
            _productRepository.Save();


            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        //    // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_productRepository.GetProductContext() == null)
            {
                return NotFound();
            }
            var product = _productRepository.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            _productRepository.DeleteProduct(product);

            _productRepository.Save();

            return NoContent();
        }


    }
}
