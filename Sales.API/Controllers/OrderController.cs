using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sales.API.DataAccess;
using Sales.API.DataAccessNoSql;
using Sales.API.Models;
using Sales.API.ViewModels;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IDataAccessNoSql<Order> _orderDataAccess;
        private readonly IDataAccessNoSql<Customer> _customerDataAccess;
        private readonly IDataAccessNoSql<Item> _itemDataAccess;

        public OrderController(IDataAccessNoSql<Order> orderDataAccess, IDataAccessNoSql<Customer> customerDataAccess, IDataAccessNoSql<Item> itemDataAccess)
        {
            this._orderDataAccess = orderDataAccess;
            this._customerDataAccess = customerDataAccess;
            this._itemDataAccess = itemDataAccess;
        }

        //RECUPERAR UM PEDIDO
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var model = await _orderDataAccess.GetAsync(Id);

            if(model == null)
                return NotFound();

            return Ok(model);
        }
        //RECUPERAR OS PEDIDOS
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var model = await _orderDataAccess.GetManyAsync();
            if(model == null)
                return NotFound();
                
            return Ok(model);
        }

        //INSERIR UM PEDIDO
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _customerDataAccess.GetAsync(inputModel.CustomerId);

            if(customer == null)
                return NotFound(customer);
            
            var model = new Order(inputModel.Vendor, customer);

            foreach (var itemDict in inputModel.Items)
            {
                var item = await _itemDataAccess.GetAsync(itemDict.Key);
                
                if(item == null)
                    return NotFound(item);

                if(itemDict.Value <= 0)
                    return BadRequest("The value of Item cannot be less than zero");

                model.AddOrderItem(new OrderItem(item, itemDict.Value));
            }

            if(model.Total == 0)
                    return BadRequest("The value of Order cannot be less than zero");

            await _orderDataAccess.CreateAsync(model);

            return Ok(inputModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string Id,[FromBody] OrderInputModel inputOrder)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var customer = await _customerDataAccess.GetAsync(inputOrder.CustomerId);
            
            if(customer == null)
                return NotFound(customer);
                
            var model = new Order(inputOrder.Vendor, customer);

            foreach(var itemDic in inputOrder.Items)
            {
                var item = await _itemDataAccess.GetAsync(itemDic.Key);

                if(item == null || itemDic.Value <= 0)
                    return NotFound(item);

                model.AddOrderItem(new OrderItem(item, itemDic.Value));
            }
            
            if(model.Total == 0)
                return NotFound(model);

            var modelUpdate = await _orderDataAccess.UpdateAsync(Id, model);
            
            if(modelUpdate == null)
                return NotFound();
            
            return Ok(model);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            await _orderDataAccess.DeleteAsync(Id);
            return NoContent();
        }
    }
}