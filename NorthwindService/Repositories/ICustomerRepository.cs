﻿using Packt.CS7;
using System.Collections.Generic;
using System.Threading.Tasks;

// Why do we need an interface? These are the functions that the service expects?
// Is it so it can be used throughout the program?




namespace NorthwindService.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateAsync(Customer c);

        Task<IEnumerable<Customer>> RetrieveAllAsync();

        Task<Customer> RetrieveAsync(string id);

        Task<Customer> UpdateAsync(string id, Customer c);

        Task<bool> DeleteAsync(string id);
    }
}