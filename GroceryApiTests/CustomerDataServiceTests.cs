using GroceryStoreAPI.External;
using GroceryStoreAPI.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GroceryApiTests
{
    public class CustomerDataServiceTests
    {
        private Mock<ILogger<CustomerDataService>> _mockLogger = new Mock<ILogger<CustomerDataService>>();
        private Database _testDatabaseEmpty = new Database()
        {
            Customers = new List<Customer>()
        };

        private List<Customer> CustomersList()
        {
            var customers = new List<Customer>();
            customers.Add(new Customer(1, "Brian"));
            customers.Add(new Customer(2, "Nicole"));
            customers.Add(new Customer(3, "Allison"));
            customers.Add(new Customer(4, "Dan Craig"));

            return customers;
        }

        private Database TestDatabaseFilled()
        {
            var database = new Database();
            database.Customers = CustomersList();
            return database;
        }

        [Fact]
        public async Task ListAllCustomersTest()
        {
            var databaseRepoMock = new Mock<IDatabaseRepository>();
            databaseRepoMock.Setup(repo => repo.ReadDatabase().Result).Returns(TestDatabaseFilled);
            var customerDataService = new CustomerDataService(databaseRepoMock.Object, _mockLogger.Object);

            var customers = await customerDataService.ListAllCustomers();
            Assert.NotEmpty(customers);
        }
        
        [Fact]
        public async Task GetCustomerSuccess()
        {
            var databaseRepoMock = new Mock<IDatabaseRepository>();
            databaseRepoMock.Setup(repo => repo.ReadDatabase().Result).Returns(TestDatabaseFilled);
            var customerDataService = new CustomerDataService(databaseRepoMock.Object, _mockLogger.Object);

            var customer = await customerDataService.GetCustomer(1);
            Assert.NotNull(customer);
        }

        [Fact]
        public async Task GetCustomerKeyNotFound()
        {
            var databaseRepoMock = new Mock<IDatabaseRepository>();
            databaseRepoMock.Setup(repo => repo.ReadDatabase().Result).Returns(TestDatabaseFilled);
            var customerDataService = new CustomerDataService(databaseRepoMock.Object, _mockLogger.Object);

            await Assert.ThrowsAnyAsync<KeyNotFoundException>(async () => await customerDataService.GetCustomer(-999));
        }

        [Fact]
        public async Task AddCustomerSuccess()
        {
            var databaseRepoMock = new Mock<IDatabaseRepository>();
            databaseRepoMock.Setup(repo => repo.ReadDatabase().Result).Returns(TestDatabaseFilled);
            databaseRepoMock.Setup(repo => repo.SaveDatabase(TestDatabaseFilled()));
            var customerDataService = new CustomerDataService(databaseRepoMock.Object, _mockLogger.Object);

            var customer = await customerDataService.AddCustomer("Barbara");
            Assert.Equal(5,customer.Id);
            Assert.Equal("Barbara", customer.Name);
        }

        [Fact]
        public async Task AddCustomerNoDatabase()
        {
            var databaseRepoMock = new Mock<IDatabaseRepository>();
            databaseRepoMock.Setup(repo => repo.ReadDatabase().Result);
            databaseRepoMock.Setup(repo => repo.SaveDatabase(TestDatabaseFilled()));
            var customerDataService = new CustomerDataService(databaseRepoMock.Object, _mockLogger.Object);

            await Assert.ThrowsAnyAsync<Exception>(async () => await customerDataService.AddCustomer("BadTest"));
        }

        [Fact]
        public async Task AddCustomerNewList()
        {
            var databaseRepoMock = new Mock<IDatabaseRepository>();
            databaseRepoMock.Setup(repo => repo.ReadDatabase().Result).Returns(_testDatabaseEmpty);
            databaseRepoMock.Setup(repo => repo.SaveDatabase(_testDatabaseEmpty));
            var customerDataService = new CustomerDataService(databaseRepoMock.Object, _mockLogger.Object);

            var customer = await customerDataService.AddCustomer("Barbara");
            Assert.Equal(1, customer.Id);
            Assert.Equal("Barbara", customer.Name);
        }

        [Fact]
        public async Task UpdateCustomerSuccess()
        {
            var databaseRepoMock = new Mock<IDatabaseRepository>();
            databaseRepoMock.Setup(repo => repo.ReadDatabase().Result).Returns(TestDatabaseFilled);
            databaseRepoMock.Setup(repo => repo.SaveDatabase(TestDatabaseFilled()));
            var customerDataService = new CustomerDataService(databaseRepoMock.Object, _mockLogger.Object);

            var customer = await customerDataService.UpdateCustomer(new Customer(3,"Barbara"));
            Assert.Equal(3, customer.Id);
            Assert.Equal("Barbara", customer.Name);
        }

        [Fact]
        public async Task UpdateCustomerNoDatabase()
        {
            var databaseRepoMock = new Mock<IDatabaseRepository>();
            databaseRepoMock.Setup(repo => repo.ReadDatabase().Result);
            databaseRepoMock.Setup(repo => repo.SaveDatabase(TestDatabaseFilled()));
            var customerDataService = new CustomerDataService(databaseRepoMock.Object, _mockLogger.Object);

            await Assert.ThrowsAnyAsync<Exception>(async () => await customerDataService.UpdateCustomer(new Customer(3, "Barbara")));
        }

        [Fact]
        public async Task UpdateCustomerNoCustomers()
        {
            var databaseRepoMock = new Mock<IDatabaseRepository>();
            databaseRepoMock.Setup(repo => repo.ReadDatabase().Result);
            databaseRepoMock.Setup(repo => repo.SaveDatabase(TestDatabaseFilled()));
            var customerDataService = new CustomerDataService(databaseRepoMock.Object, _mockLogger.Object);

            await Assert.ThrowsAnyAsync<Exception>(async () => await customerDataService.UpdateCustomer(new Customer(3, "Barbara")));
        }

        [Fact]
        public async Task UpdateCustomerCustomerNotFound()
        {
            var databaseRepoMock = new Mock<IDatabaseRepository>();
            databaseRepoMock.Setup(repo => repo.ReadDatabase().Result);
            databaseRepoMock.Setup(repo => repo.SaveDatabase(TestDatabaseFilled()));
            var customerDataService = new CustomerDataService(databaseRepoMock.Object, _mockLogger.Object);

            await Assert.ThrowsAnyAsync<Exception>(async () => await customerDataService.UpdateCustomer(new Customer(-999, "Barbara")));
        }
    }
}
