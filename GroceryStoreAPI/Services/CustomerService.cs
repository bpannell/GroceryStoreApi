using GroceryStoreAPI.External;
using GroceryStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerDataService _customerDataService;

        public CustomerService(ICustomerDataService customerDataService)
        {
            _customerDataService = customerDataService;
        }

        public async Task<List<Customer>> ListAllCustomers()
        {
            return await _customerDataService.ListAllCustomers();
        }

        public async Task<Customer> GetCustomer(int id)
        {
            return await _customerDataService.GetCustomer(id);
        }

        public async Task<Customer> AddCustomer(string name)
        {
            return await _customerDataService.AddCustomer(name);
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            return await _customerDataService.UpdateCustomer(customer);
        }
    }
}
