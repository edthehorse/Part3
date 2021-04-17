using Microsoft.EntityFrameworkCore.ChangeTracking;
using Packt.CS7;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//                      DICTIONARY OPERATIONS

namespace NorthwindService.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        // Cache the customers in a thread-safe dictionary to improve performance.
        private static ConcurrentDictionary<string, Customer> customersCache;

        private Northwind db;// The database context.

        public CustomerRepository(Northwind db) // 
        {
            this.db = db;

            LoadCache();  // Load up the dictionary (see below) which uses db. ed

        } // Constructor -  dep injection of db context. ed

        



        // LOADS THE CUSTOMERS TABLE FROM THE DATABASE INTO THE DICTIONARY
        private void LoadCache() // Dictionary <-> Cache.
        {
            // Pre-load customers from database as a normal Dictionary (using .ToDictionary) with CustomerID as the key,  
            // then convert to a thread-safe ConcurrentDictionary 

            if (customersCache == null) // 91 key value pairs. ed.
            { // If the customersCache dictionary is null then load it up.
                try
                {
                    customersCache = new ConcurrentDictionary<string, Customer>( // Make a new dictionary and ..
                    db.Customers.ToDictionary(c => c.CustomerID));               // ... load it up with customers with that Id.
                }
                catch { }
            }
        } // Loads Dictionary
      
 



    
        // ------- THESE FUNCTIONS ARE FOR THE CACHE (as specied by the interface ICustomerRepository) -------------------------
        public async Task<Customer> CreateAsync(Customer c)  // Custom method : CreateAsync
        {
            c.CustomerID = c.CustomerID.ToUpper();// Convert CustomerID into uppercase and compare. ed

            // Add to the database using EF Core
            EntityEntry<Customer> added = await db.Customers.AddAsync(c); // A DBSet method : AddAsync 

            int affected = await db.SaveChangesAsync(); // // A DBSet method : SaveChangesAsync

            if (affected == 1)
            {
                // If the customer is new, add it to cache, else call UpdateCache method 
                return customersCache.AddOrUpdate(c.CustomerID, c, UpdateCache);
            }

            else
            {
                return null;
            }
        }

        //                              ALL
        public async Task<IEnumerable<Customer>> RetrieveAllAsync()   // For performance-wise, get (all)from cache
        {
            LoadCache(); // See above. Loads up the dictionary from the dbs.
           
            return await Task.Run<IEnumerable<Customer>>( () => customersCache.Values); // A new thread ? Gets all 91 customers. Values is all of the customers. 

        } // Custom method : RetrieveAllAsync

        //                              ONE
        public async Task<Customer> RetrieveAsync(string id) // From the customersCache dictionary. // For performance-wise, get from cache
        {
            return await Task.Run(() => // Runs a separate thread?
            {   
                id = id.ToUpper();
                Customer c; // Empty object for the customer to be retrieved.
                customersCache.TryGetValue(id, out c); // The customer c with that Id will (hopefully) be returned (by out reference).
                return c;  // Return a Customer !
            });
        }


        public async Task<Customer> UpdateAsync(string id, Customer c)// UpdateCache is our dictionary method. ed
        {
            return await Task.Run(() =>
            {
                id = id.ToUpper();// normalize customer ID ie to uppercase

                c.CustomerID = c.CustomerID.ToUpper(); // and this.

                // Update in database

                db.Customers.Update(c);

                int affected = db.SaveChanges();

                if (affected == 1) // Then update in cache
                {
                    return Task.Run(() => UpdateCache(id, c));// UpdateCache is a dictionary method. ed
                }

                return null;
            });
        }


        //                              DELETE          from the dbs AND the cache.
        public async Task<bool> DeleteAsync(string id)// Deletes the customer with Id 4 from the database AND from the dictionary cache.
        {
            return await Task.Run(() =>
            {
                id = id.ToUpper();

                // Remove from database but..

                Customer c = db.Customers.Find(id); // First find the customer with that ID. ed

                db.Customers.Remove(c); // ... and remove it from the actual dbs. ed

                int affected = db.SaveChanges(); // Dont forget. ed

                if (affected == 1) // If ONE was deleted from the dbs then ...
                {
                    return Task.Run(() => customersCache.TryRemove(id, out c));// ... remove from customersCache cache as well ie. ed
                }

                else
                {
                    return null; // If it wasnt found then exit. ed
                }
            });
        }



        // Extra ? -----------------------------------------------------------------


        //                                  CACHE UPDATE                only.
        private Customer UpdateCache(string id, Customer c) //  Doesn't update the dbs (nor its context!) but rather the cache dictionary.
        { 
            // c is the new customer to replace old.
            Customer old; // Empty object will be used to retrieve customer with that Id.

            if (customersCache.TryGetValue(id, out old)) // Try to retrieve the customer with that id. If successfull will be returned in old.
                                                         // TryGetValue is a method of the dictionary.
            {
                if (customersCache.TryUpdate(id, c, old))// So is TryUpdate. So replace customer old with c. 
                {
                    return c; // If successful then return the customer we input!
                }
            }

            return null;
        }
    }
} 