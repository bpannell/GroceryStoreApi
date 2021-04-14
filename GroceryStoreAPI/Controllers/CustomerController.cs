using GroceryStoreAPI.Models;
using GroceryStoreAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger _logger;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> AllCustomers()
        {
            try
            {
                _logger.LogInformation("Entering AllCustomers Call");
                return Ok(await _customerService.ListAllCustomers());
            }
            catch (Exception e)
            {
                _logger.LogError($"{e}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Customer(int id)
        {
            try
            {
                return Ok(await _customerService.GetCustomer(id));
            }
            catch (KeyNotFoundException nfr)
            {
                _logger.LogError($"{nfr}");
                return BadRequest(new BadRequestObjectResult(nfr.Message));
            }
            catch (Exception e)
            {
                _logger.LogError($"{e}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> AddCustomer(string name)
        {
            try
            {
                return Ok(await _customerService.AddCustomer(name));
            }
            catch (Exception e)
            {
                _logger.LogError($"{e}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(Customer customer)
        {
            try
            {
                return Ok(await _customerService.UpdateCustomer(customer));
            }
            catch (Exception e)
            {
                _logger.LogError($"{e}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
