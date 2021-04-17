using Microsoft.AspNetCore.Mvc;
using NorthwindMvc.Models;
using Packt.CS7;
using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace NorthwindMvc.Controllers
{
    public class HomeController : Controller
    {
        private Northwind db;

        public HomeController(Northwind injectedContext) 
        {
            db = injectedContext; // db is now available for the whole program ed 
        }
        
        // STARTS HERE
        public IActionResult Index()
        {
            var model = new HomeIndexViewModel // This loads up model object with all Categories etc. ed
            {
                VisitorCount = (new Random()).Next(1, 1001), // Will be same random number to start? ed 
                Categories  = db.Categories.ToList(),        // Categories is a LIST of categories.
                Products = db.Products.ToList()              // Products is a LIST of products.
            };

            return View(model); // Pass model to view . Take a look at Index.cshtml in the HOME folder. ed
            // OK its called model but it contains stuff as well so we call it a VIEWmodel. ed.
        }

        public IActionResult About()       // ABOUT ed
        {
            ViewData["Message"] = "Your application description page.";

            return View(); // Simply returns the About page. No model passing
        } 

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View(); // Simply returns the Contact page. No model passing
        }  // CONTACT ed

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }    // ERROR ed

        public IActionResult ProductDetail(int? id) // Called (only) from the Home/Index.cshtml view. ed /Home/ProductDetail/50
        
        {
            if (!id.HasValue)  // ie it DOESNT have a value.
            {
                return NotFound("You must pass a product ID in the route, for example, /Home/ProductDetail/21");
            }

            // ie If it DOES have a value:
            var model = db.Products.SingleOrDefault(p => p.ProductID == id); // Get the Product with that Id. ed

            if (model == null)
            {
                return NotFound($"A product with the ID of {id} was not found.");
            }

            return View(model); // Pass model(containing one product: Chef Anton's ...) to Home/PRODUCTDETAIL view. ed 
        }


        // STEP 2 Receive the Price value and get the product details. ed 
        public IActionResult ProductsThatCostMoreThan(decimal? price) // eg 50 ed
        {
            if (!price.HasValue)
            {
                return NotFound("You must pass a product price in the query string, for example, /Home/ProductsThatCostMoreThan?price=50"); 
            }

            var model = db.Products.Include(p => p.Category).Include( p => p.Supplier).Where(p => p.UnitPrice > price).ToArray();
            // Gets products whose price is > 50 BUT gets the respective Category AND Supplier ie. ed. 
            // Category Id      : 6                     // Address          :   
            // Category Name    : Meat/Poultry          // City             : 
            // Description      : Prepared Meats        // SupplierName     : 
            // Products         : Count= 3              // etc              : 

            if (model.Count() == 0) // This var ,model variable has been modified => to the Categories more than say 50. ed 
            {
                return NotFound($"No products cost more than {price:C}.");
            }

            ViewData["MaxPrice"] = price.Value.ToString("C");

        // STEP 3 Send the model containing the 8 products (and their respective Categories & Suppliers 
        // (and the ViewData data)to the Home/ProductsThatCostMoreThan view. ed
            return View(model); // Pass model to view 
        }
    }
}
