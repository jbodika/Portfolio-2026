using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using jbH60Customer.Models;
using Microsoft.AspNetCore.Identity;

namespace jbH60Customer.Controllers
{
    public class OrdersController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(HttpClient httpClient, UserManager<IdentityUser> userManager)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5076/api/");
            _userManager = userManager;

        }
        // GET: OrdersController
        public ActionResult Index()
        {
            return View();
        }






        public async Task<IActionResult> DisplayOrderSummary()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var shoppingCart = await _httpClient.GetFromJsonAsync<ShoppingCart>($"ShoppingCarts/CartsByEmail/{currentUser.Email}");
            var customer = await _httpClient.GetFromJsonAsync<Customer>($"Customers/CustomerByEmail/{currentUser.Email}");
            var cost = await _httpClient.GetFromJsonAsync<double>($"CartItems/TotalPrice/{currentUser.Email}");
            var responseTax = await _httpClient.PostAsJsonAsync($"Orders/CalculateTax/{cost}/{customer.Province}", shoppingCart);
            var cartItems = await _httpClient.GetFromJsonAsync<List<CartItem>>($"CartItems/{currentUser.Email}");
            decimal taxes = 0;
            if (responseTax.IsSuccessStatusCode)
            {
                taxes = Decimal.Parse(await responseTax.Content.ReadAsStringAsync());
            }

           
            if (shoppingCart != null)
            {
                var order = new Order
                {
                    CustomerId = customer.CustomerId,
                    Taxes = taxes,
                    DateCreated = DateTime.Now,
                    DateFufilled = DateTime.Now,
                    Total = (decimal)cost,
                    Customer = customer,


                };
                var orderResp = await _httpClient.PostAsJsonAsync("Orders", order);

                if (orderResp.IsSuccessStatusCode)
                {
                    

                    var orderId = await _httpClient.GetFromJsonAsync<int>("Orders/LastOrderID");

                    foreach (var item in cartItems)
                    {
                        var orderItem = new OrderItem
                        {
                            OrderId = orderId,
                            Price = item.Price,
                            Product = item.Product,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity
                        };

                        var orderItemResp = await _httpClient.PostAsJsonAsync("OrderItems", orderItem);

                        if (orderItemResp.IsSuccessStatusCode)
                        {



                            await _httpClient.DeleteAsync($"CartItems/OrderCompletion/{item.CartItemId}");
                        }

                    }
                    var cartDel = await _httpClient.DeleteAsync($"ShoppingCarts/{shoppingCart.CartId}");


                    if (cartDel.IsSuccessStatusCode)
                    {

                        return View("OrderSummary", order);
                    }

                    return View("OrderSummary");
                }
            }
            return View("OrderSummary");
        }





        public async Task<IActionResult> MakePayment(string creditcard)
        {

            var currentUser = await _userManager.GetUserAsync(User);
            var shoppingCart = await _httpClient.GetFromJsonAsync<ShoppingCart>($"ShoppingCarts/CartsByEmail/{currentUser.Email}");
            var customer = await _httpClient.GetFromJsonAsync<Customer>($"Customers/CustomerByEmail/{currentUser.Email}");
            var cost = await _httpClient.GetFromJsonAsync<double>($"CartItems/TotalPrice/{currentUser.Email}");
            var responseTax = await _httpClient.PostAsJsonAsync($"Orders/CalculateTax/{cost}/{customer.Province}", shoppingCart);
            var cartItems = await _httpClient.GetFromJsonAsync<List<CartItem>>($"CartItems/{currentUser.Email}");

            if (customer.CreditCard != null)
            {
                 var checkCreditCard = await _httpClient.GetFromJsonAsync<int>($"Orders/CreditCard/{creditcard}");

                if (checkCreditCard ==0)
                {
                   return await DisplayOrderSummary();
                }
                else if(checkCreditCard == -1)
                {
                    TempData["CreditCardError"] = "Your credit card has an invalid length!";
                }

                else if(checkCreditCard == -2)
                {
                    TempData["CreditCardError"] = "There's a mix of non digits in there!";

                }

                else if(checkCreditCard == -3)
                {
                    TempData["CreditCardError"] = " sum of each 4 < 30";

                }
                else if(checkCreditCard == -4)
                {
                    TempData["CreditCardError"] = "product of last 2 digits must be multiple of 2";

                }
                else if(checkCreditCard ==-5)
                {
                    TempData["CreditCardError"] = "Insufficient balance!";

                }
              
             
            }


            return RedirectToAction("ProcessPayment", "ShoppingCarts");

        }







        //var order = new Order
        //{
        //    CustomerId = customer.CustomerId,
        //    Taxes = taxes,
        //    DateCreated = DateTime.Now,
        //    DateFufilled = DateTime.Now,
        //    Total = (decimal)cost,



        //};

        //var response = await _httpClient.PostAsJsonAsync("Orders",order);

        //if (response.IsSuccessStatusCode)
        //{
        //   var orderObj=  response.Content.ReadAsStringAsync();

        //   response.
        //}
        //else
        //{
        //    return View("Error");
        //}

        //foreach (var cartItem in shoppingCart.CartItems)
        //{
        //    var orderItem = new OrderItem
        //    {
        //        OrderId
        //        ProductId = cartItem.ProductId,
        //        Quantity = cartItem.Quantity,
        //        Price = cartItem.Price,

        //        // other order item properties...
        //    };

        //    order.OrderItems.Add(orderItem);
        //}

        //currentUser.Email;



        //_httpClient.PostAsJsonAsync<OrderItem>("OrderItems",orderItem);
        // OrderItem orderItem = new OrderItem({ });




        // _httpClient.PostAsJsonAsync("", );

        // GET: OrdersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrdersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrdersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrdersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrdersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrdersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrdersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
