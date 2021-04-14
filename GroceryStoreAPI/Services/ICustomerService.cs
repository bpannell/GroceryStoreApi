using GroceryStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> ListAllCustomers();
        Task<Customer> GetCustomer(int id);
        Task<Customer> AddCustomer(string name);
        Task<Customer> UpdateCustomer(Customer customer);
    }
}
