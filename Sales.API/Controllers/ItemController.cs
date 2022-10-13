using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Sales.API.DataAccess;
using Sales.API.DataAccessNoSql;
using Sales.API.Models;
using Sales.API.ViewModels;

//arquitetura MVC: model, controller e view
//model -> modelo representativo de dados atraves de objetos/classes
//view -> basicamente, esta na linha de frente da comunicacao com o usuario final do sistema
//controller -> intermediacao entre a view e as regras de negocios, com o fornecimento de model ou ate mesmo respondendo o model

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ItemDataNoSql itemDataAccess;

        public ItemController(ItemDataNoSql itemDataAccess)
        {
            this.itemDataAccess = itemDataAccess;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await itemDataAccess.GetItemsAsync();

            return Ok(items);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var item = await itemDataAccess.GetItemAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ItemInputModel itemInputModel)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = new Item(itemInputModel.Name, itemInputModel.Price);

            await itemDataAccess.CreateItemAsync(model);

            return CreatedAtAction(nameof(Get), new {id = model.Id} ,itemInputModel);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] ItemInputModel itemInput)
        {
            var item = new Item(itemInput.Name,itemInput.Price);
            item.Id = id;
            var model = await itemDataAccess.UpdateItemAsync(id, item);

            if (model != null)
                return Ok(model);

            return NotFound();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await itemDataAccess.DeleteItemAsync(id);
            return NoContent();
        }
    }
}