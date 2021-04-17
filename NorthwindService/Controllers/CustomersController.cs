using Microsoft.AspNetCore.Mvc;
using Packt.CS7;
using NorthwindService.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



// Summary. A dictionary customersCacheis created which mimicks dbs.
// repo is an object which deals with all of hte functionaility of that process.

// The customers seem to be done more elaborately than the eg Categories. 

namespace NorthwindService.Controllers
{
    // base address: api/customers          ALL ? No See MANY below.
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private ICustomerRepository repo; // ICustomerRepository is our custom interface that repos must now follow.
        // repo is a class of ICustomerRepository type. Injected into this constructor. See below.

        // Constructor INJECTS registered repository   - as distinct from the database. 
        // So here's the point of DI. We formulate the service and THEN send it 
        // I guess it must be an interface if its going to be injected.
        // Summary DI ie injecting into the constructor allows us to change whats injected ie database context of this repo. In order to be able to inject, it must be an INTERFACE.
        // As a side-effect the injected object eg db is then known throughout the program.public CustomersController(ICustomerRepository repo) // 
        public CustomersController(ICustomerRepository repo) // 
                                                             // repo is a CustomerRepository object (: ICustomerRepository) and as such
                                                             // will call the methods in CustomerRepository.cs which deals with the dictionay operations on customersCache. 
        {
            this.repo = repo;
        }

        // ---------------- GETS ------------------------------

        // GET: api/customers eg api/customers
        // GET: api/customers/?country=[country]  api/customers/3


        [HttpGet]//                     MANY                customers of ONE country
        public async Task<IEnumerable<Customer>> GetCustomers(string country) // Get all of the customers for a particular country.
        {
            if (string.IsNullOrWhiteSpace(country))    // If no country specified then ...
            {
                return await repo.RetrieveAllAsync();  // ... get ALL of the countries. ed
            }
            else
            {
                return (await repo.RetrieveAllAsync()) // RetrieveAllAsync is just a function 
                    .Where(customer => customer.Country == country); // Where the customer is of the particualr country eg Germany. ed
            }
        }

        //                              ONE                 customer
        // GET: api/customers/[id] eg api/customers/4
        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<IActionResult> GetCustomer(string id)
        {
            Customer c = await repo.RetrieveAsync(id); // Get eg customer 3. ed

            if (c == null) // If no customer returned.. ed
            {
                return NotFound(); // 404 Resource not found ed
            }

            return new ObjectResult(c); // If a customer found then .. 200 OK  ed
        }

        // ---------------- POSTS ------------------------------

        // POST: api/customers 
        // BODY: Customer (JSON, XML)
        
        [HttpPost]//                    ADD                 customer
        public async Task<IActionResult> Create([FromBody] Customer c) // Customer object is added to the database repo. ed
        {
            if (c == null)// If no customer was specified  then ...  
            {
                return BadRequest(); // ... finish up. ed // 400 Bad request 
            }  

            Customer added = await repo.CreateAsync(c); // Create a customer? ed
           
            return CreatedAtRoute("GetCustomer", new { id = added.CustomerID.ToLower() }, c); // use named route// 201 Created 
        }

        //                              EDIT            
        // PUT: api/customers/[id] 
        // BODY: Customer (JSON, XML) 
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Customer c)
        {
            id = id.ToUpper();                      // Convert this Id to upper case. ed (Will this cause error if null?)
            c.CustomerID = c.CustomerID.ToUpper();  // and this.

            if (c == null || c.CustomerID != id) // If the Customer Id is null or there is no match..
            {
                return BadRequest(); // ... exit. ed. 400 Bad request 
            } // If no Id or null..

            var existing = await repo.RetrieveAsync(id); // Otherwise retrieve the customer with that Id. 
            // RetrieveAsync is our custom method in CustomerRepository

            if (existing == null) // If customer not found....
            {
                return NotFound(); // ... then exit ed404 Resource not found 
            } // If nothing found.

            await repo.UpdateAsync(id, c); // Updates cache as well! ed
            // Updatesync is our custom method in CustomerRepository

            return new NoContentResult(); //  204 No content . A Task<IActionResult> is returned.
        }



        //                              DELETE          a customer
        // DELETE: api/customers/[id] 
        [HttpDelete("{id}")] // Note the Delete in HttpDelete
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await repo.RetrieveAsync(id); // Gets the object with that id from the cache. ed
            // RetrieveAsync is our custom method in CustomerRepository

            if (existing == null)
            {
                return NotFound(); // 404 Resource not found 
            } // If no customer exists with that Id then exit. ed.

            bool deleted = await repo.DeleteAsync(id); // Presumably TRIES to delete. If successful then deleted will refer to that deleted object. 
            // DeleteAsync is our custom method in CustomerRepository

            if (deleted) // ie if not null ...
            {
                return new NoContentResult(); // 204 No content 
            } // ie we have an object returned above so it must have been deleted.
           
            else
            {
                return BadRequest(); // If it was null ie nothing deletecd then return BadRequest.
            }
        }




    }
}