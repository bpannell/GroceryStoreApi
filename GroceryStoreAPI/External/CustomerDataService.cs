using GroceryStoreAPI.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.External
{
    public class CustomerDataService : ICustomerDataService
    {
        private readonly IDatabaseRepository _databaseRepository;
        private readonly ILogger _logger;

        public CustomerDataService(IDatabaseRepository databaseRepository, ILogger<CustomerDataService> logger)
        {
            _databaseRepository = databaseRepository;
            _logger = logger;
        }

        public async Task<List<Customer>> ListAllCustomers()
        {
            var database = await _databaseRepository.ReadDatabase();
            _logger.LogDebug($"Returned {database?.Customers?.Count} Customers");
            return database?.Customers;
        }

        public async Task<Customer> GetCustomer(int id)
        {
            var customers = await ListAllCustomers();
            var customer = customers.FirstOrDefault(x => x.Id == id);

            if (customer == null)
            {
                _logger.LogWarning($"No Customer was found for id: {id}");
                throw new KeyNotFoundException("There is no customer with that given Id");
            }
            _logger.LogDebug($"Customer {customer} was found with id: {id}");
            return customer;
        }

        public async Task<Customer> AddCustomer(string name)
        {
            var database = await _databaseRepository.ReadDatabase();
            Customer customer;
            
            if (database == null)
            {
                throw new Exception("Database Could Not Be Loaded");
            }
            if (database.Customers == null || database.Customers?.Count == 0)
            {
                _logger.LogDebug($"No Customers were found in the database, so adding {name} as the first");
                database.Customers = new List<Customer>();
                customer = new Customer(1, name);
                database.Customers.Add(customer);
            }
            else
            {
                customer = new Customer(database.Customers.Max(x => x.Id) + 1, name);
                _logger.LogDebug($"Customer {name} is being added to the List with id {customer.Id}");
                database.Customers.Add(customer);
            }

            try
            {
                await _databaseRepository.SaveDatabase(database);
                _logger.LogDebug($"Customer {customer} added to the database");
                return customer;
            }
            catch (Exception e)
            {
                _logger.LogWarning($"{e}");
                throw;
            }
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            var database = await _databaseRepository.ReadDatabase();
            if (database == null)
            {
                throw new Exception("Database Could Not Be Loaded");
            }

            var customers = database.Customers;
            if (customers == null || customers.Count == 0)
            {
                _logger.LogWarning($"{customer.Id} does not exist in the database to update");
                throw new Exception("There are currently no Customers to update");
            }
            if (!customers.Any(x => x.Id == customer.Id))
            {
                _logger.LogWarning($"{customer.Id} does not exist in the database to update");
                throw new Exception($"No Customer with id: {customer.Id} currently exists to update");
            }
            customers.FirstOrDefault(x => x.Id == customer.Id)?.UpdateCustomer(customer);
            _logger.LogDebug($"Customer {customer.Id} is having the name updated to {customer.Name}");
            database.Customers = customers;            

            try
            {
                await _databaseRepository.SaveDatabase(database);
                _logger.LogDebug($"Customer {customer} updated in the database");
                return customer;
            }
            catch (Exception e)
            {
                _logger.LogWarning($"{e}");
                throw;
            }
        }
    }
}
