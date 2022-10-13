using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Sales.API.Models;

namespace Sales.API.DataAccessNoSql
{
    public class ItemDataNoSql
    {
        private readonly IMongoCollection<Item> _collection;

        public ItemDataNoSql(IOptions<SalesDatabaseSetting> salesDatabaseSetting)
        {
            var mongoClient = new MongoClient(salesDatabaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(salesDatabaseSetting.Value.DatabaseName);
            _collection = mongoDatabase.GetCollection<Item>(salesDatabaseSetting.Value.ItemCollection);
        }

        public async Task<List<Item>> GetItemsAsync() => await _collection.Find(x => true).ToListAsync();

        public async Task<Item?> GetItemAsync(string id) => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        
        public async Task CreateItemAsync(Item item) => await _collection.InsertOneAsync(item);

        public async Task<Item> UpdateItemAsync(string id, Item item)
        {
            var findItem = _collection.Find(x=>x.Id == id).FirstOrDefaultAsync();
            if(findItem == null)
                return null;
                
            await _collection.ReplaceOneAsync(x=>x.Id == id, item);
            return item;
        }

        public async Task DeleteItemAsync(string id)
        {
            await _collection.DeleteOneAsync(x=>x.Id == id);
        }
    }
}