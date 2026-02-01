using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jbH60Services.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreLibrary.DAL;
using StoreLibrary.DAL.Repositories;
using StoreLibrary.DTO;
using StoreLibrary.Models;

namespace jbH60Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryRepository _prodCatRepository;
        public ProductCategoriesController(IProductCategoryRepository prodCatRepository)
        {

            _prodCatRepository = prodCatRepository;


        }


        // GET: api/ProductCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategories()
        {
          if (_prodCatRepository.GetProdCatContext() == null)
          {
              return NotFound();
          }
            return await _prodCatRepository.GetProdCatContext().ToListAsync();
        }


        [HttpGet("SortedAlphabetically")]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> SortedAlphabetically()
        {
            if (_prodCatRepository.SortAlphabetically() == null)
            {
                return NotFound();
            }
            return  Ok(_prodCatRepository.SortAlphabetically().ToList());
        }


        [HttpGet("Count/{id}")]
        public async Task<ActionResult<int>> ProductCategoriesCount(int id)
        {
             return Ok(_prodCatRepository.ProductCategoriesCount(id));
            // return Ok(_prodCatRepository.ProductCountByProductCategory(id));
        }

        [HttpGet("AllProductCategoriesCount")]
        public async Task<ActionResult<int>> AllProductCategoriesCount()
        {
            return Ok(_prodCatRepository.AllProductCategoriesCount());
            // return Ok(_prodCatRepository.ProductCountByProductCategory(id));
        }

        [HttpGet("AllProductCategoriesCount/{searchName}")]
        public async Task<ActionResult<int>> AllProductCategoriesCountBasedOnSearch(string searchName)
        {
            return Ok(_prodCatRepository.CountOfProdCats(searchName));
            // return Ok(_prodCatRepository.ProductCountByProductCategory(id));
        }



        // GET: api/ProductCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> GetProductCategory(int id)
        {
          if (_prodCatRepository.GetProdCatContext() == null)
          {
              return NotFound();
          }
            var productCategory = await _prodCatRepository.GetProdCatContext().FindAsync(id);

            if (productCategory == null)
            {
                return NotFound();
            }

            return productCategory;
        }

        // PUT: api/ProductCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCategory(int id, ProductCategory productCategory)
        {
            if (id != productCategory.CategoryId)
            {
                return BadRequest();
            }

           
            try
            {
                _prodCatRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_prodCatRepository.ProductCategoryExists(id))
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

        [HttpGet("ProductCategorySearch/{prodCatName}")]
        public async Task<ActionResult<List<ProductCategory>>> ProductCategorySearch(string prodCatName)
        {
            var product = _prodCatRepository.PartialProductCategorySearch(prodCatName);

            if (product == null)
            {
                return NotFound(prodCatName);

            }
            return Ok(product);
        }


        // POST: api/ProductCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductCategory>> PostProductCategory(ProductCategory productCategory)
        {
          if (_prodCatRepository.GetProdCatContext() == null)
          {
              return Problem("Entity set 'H60AssignmentDB_jbContext.ProductCategories'  is null.");
          }
            _prodCatRepository.AddProductCategory(productCategory);
            _prodCatRepository.Save();

            return CreatedAtAction("GetProductCategory", new { id = productCategory.CategoryId }, productCategory);
        }

        // DELETE: api/ProductCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            if (_prodCatRepository.GetProdCatContext() == null)
            {
                return NotFound();
            }
            var productCategory = await _prodCatRepository.GetProdCatContext().FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            _prodCatRepository.DeleteProductCategory(productCategory);
            _prodCatRepository.Save();

            return NoContent();
        }


    }
}
