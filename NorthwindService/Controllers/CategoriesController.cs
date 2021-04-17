using Microsoft.AspNetCore.Mvc;
using Packt.CS7;
using System.Collections.Generic;
using System.Linq;

namespace NorthwindService.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly Northwind db; // The context. Northwind class is in the library NorthWindContextLib

        //                                   DB
        public CategoriesController(Northwind db) // Inject the context. ed
        {
            this.db = db;
        }


        // GET: api/categories              ALL
        [HttpGet]
        public IEnumerable<Category> Get() // Get all of the categories. 8 of them . Beveridges, Condiments, etc.   ed
        {
            var categories = db.Categories.ToArray(); // Put them into an array called categories of categories. 8 arrays. ed. eg:
            // Category ID   : 1
            // CategoryName  :
            // Description   :  
            // Products      :

            return categories; // Not a view! Return to where?
        }



        // GET api/categories/5             ONE
        [HttpGet("{id}")]
        public Category Get(int id) // Just get that category. eg category with id 4.
        {
            var category = db.Categories.Find(id);

            return category;  // Not a view! Returns the category with that id. ed
        }
       
        // categoryID	4
        // categoryName	"Dairy Products"
        // description	"Cheeses"
        // products	null

    }
}
