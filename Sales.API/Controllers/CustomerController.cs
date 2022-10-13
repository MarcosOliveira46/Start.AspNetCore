using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Sales.API.DataAccessNoSql;
using Sales.API.Models;
using Sales.API.ViewModels;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerDataNoSql _context;

        public CustomerController(CustomerDataNoSql context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customers = await _context.GetCustomersAsync();

            return Ok(customers);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var customer = await _context.GetCustomerAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = new Customer(inputModel.Name, inputModel.Email, inputModel.Phone, inputModel.Identity);
            model.Age = inputModel.Age;            
            
            await _context.CreateCustomerAsync(model);

            return CreatedAtAction(nameof(Get),new {id = model.Id},model);
        }

        [HttpPut]
        [Route("{Id}")]
        public async Task<IActionResult> Put(string Id, [FromBody] CustomerInputModel InputCustomer)
        {
            var customer = new Customer(InputCustomer.Name, InputCustomer.Email, InputCustomer.Phone, InputCustomer.Identity);
            customer.Id = Id;
            var client = _context.UpdateCustomerAsync(Id, customer);
            if(client == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _context.DeleteCustomerAsync(id);
            return NoContent();
        }
    }
}