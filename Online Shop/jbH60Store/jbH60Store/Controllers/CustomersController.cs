using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using jbH60Store.Models;
using System.Net.Http.Json;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace jbH60Store.Controllers
{
    [Authorize(Roles ="Clerk, Manager")]
    public class CustomersController : Controller
    {
        private readonly HttpClient _httpClient;

        public CustomersController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5076/api/Customers/");
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {


            return _httpClient.GetFromJsonAsync<IEnumerable<Customer>>("") != null ?
                        View(await _httpClient.GetFromJsonAsync<IEnumerable<Customer>>("")) :
                        Problem("Entity set 'H60AssignmentDB_jbContext.Customers'  is null.");
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            string TSCustomer = id.ToString();

            var customer = await _httpClient.GetFromJsonAsync<Customer>(TSCustomer);

            if (id == 0 || customer == null)
            {
                return NotFound();
            }



            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FirstName,LastName,Email,PhoneNumber,Province,CreditCard")] Customer customer)
        {

            var response = await _httpClient.PostAsJsonAsync("", customer);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));

            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string TSCustomer = id.ToString();

            if (id == null || await _httpClient.GetFromJsonAsync<IEnumerable<Customer>>("") == null)
            {
                return NotFound();
            }

            var customer = await _httpClient.GetFromJsonAsync<Customer>(TSCustomer);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        //// POST: Customers/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FirstName,LastName,Email,PhoneNumber,Province,CreditCard")] Customer customer)
        {
            string TSCustomer = id.ToString();

            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  
                    var response = await _httpClient.PutAsJsonAsync(TSCustomer, customer);

                    if (response.IsSuccessStatusCode)
                    {
                      
                        return View("Details", customer);

                    }
                    else
                    {
                        return View(customer);

                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (customer == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(customer);


        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string TSCustomer = id.ToString();
            if (await _httpClient.GetFromJsonAsync<IEnumerable<Customer>>("") == null)
            {
                return Problem("Entity set 'H60AssignmentDB_jbContext.Customers'  is null.");
            }
            var response = await _httpClient.DeleteAsync(TSCustomer);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));

            }
            else
            {

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["ErrorMessage"] = "Customer cannot be removed due to having orders and a shopping cart";
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while deleting the product.";
                }
            }


            return RedirectToAction("Error");

        }

        //private bool CustomerExists(int id)
        //{
        //  return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        //}
    }
}
