using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Sales.API.DataAccess;
using Sales.API.Models;

namespace Sales.API.DataAccessNoSql
{
    public class OrderDataNoSql: IDataAccessNoSql<Order>
    {
        private readonly IMongoCollection<Order> _collection;

        public OrderDataNoSql(IOptions<SalesDatabaseSetting> salesDatabaseSetting)
        {
            var mongoClient = new MongoClient(salesDatabaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(salesDatabaseSetting.Value.DatabaseName);
            _collection = mongoDatabase.GetCollection<Order>(salesDatabaseSetting.Value.OrderCollection);
        }

        public async Task<List<Order>> GetManyAsync() => await _collection.Find(new BsonDocument()).ToListAsync();

        public async Task<Order?> GetAsync(string id) => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        
        public async Task CreateAsync(Order order) => await _collection.InsertOneAsync(order);

        public async Task<Order?> UpdateAsync(string Id, Order Order)
        {
            var order = await _collection.Find(x=>x.Id == Id).FirstOrDefaultAsync();

            if(order == null)
                return null;
            
            await _collection.ReplaceOneAsync(x=>x.Id == Id, Order);

            return Order;
        }

        public async Task DeleteAsync(string id) => await _collection.DeleteOneAsync(x=>x.Id == id);
    }
}