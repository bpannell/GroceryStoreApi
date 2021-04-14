using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Customer()
        {
            
        }

        public Customer(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void UpdateCustomer(Customer customer)
        {
            Name = customer.Name;
        }
    }
}
