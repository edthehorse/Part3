using Microsoft.AspNetCore.Mvc;
using Packt.CS7;
using System.Collections.Generic;
using System.Linq;




namespace NorthwindService.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller // These are not async methods?  ed no but Customers they are - for the exercise.
    {
        private readonly Northwind db; // The context. 

        public ProductsController(Northwind db) // Dep injection. 
        {
            this.db = db;
        }

        // GET: api/products
        [HttpGet]                            //      ALL
        public IEnumerable<Product> Get()    // Get all of the products ed. Get will be the default!
        {
            var products = db.Products.ToArray(); // 78 products is an array pf products. ed

            return products;
            // Category
            // Supplier etc
        }



        // GET api/products/5
        [HttpGet("{id}")]                   //      MANY
        public IEnumerable<Product> GetByCategory(int id) // Get all of the products of a particular category eg category id = 4 
        { // Can we call this (GetByCategory) whatever we like since we are using atributes? ed Changed it ti GetByCat and still works!
            // Even call it Get()

            var products = db.Products.Where(p => p.CategoryID == id).ToArray(); // 10 of them ed
            // Note that we are finding the PRODUCTS corresponding to a CATEGORY. ed

            return products;
        }



        //// GET api/delete/5  // EDs !!      //      DELETE
        //[HttpDelete("{id}")] // Note the name of this Http : Delete!
        //public Product Delete(int id) 
        //{
        //    // First get the product with that Id. ed  eg 78
        //    var product = db.Products.Where(p => p.ProductID == id); // 1 product

        //    //if (product == null)
        //    //{
        //    //    return NotFound(); // 404 Resource not found 
        //    //}

        //    db.Remove(product);  // Then remove that product.

        //    db.SaveChanges();    // Dont forget.

        //    return product; // need to return a task??
        //}



    }
}
