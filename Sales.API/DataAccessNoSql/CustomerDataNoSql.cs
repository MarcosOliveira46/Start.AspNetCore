using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sales.API.Models;

namespace Sales.API.DataAccessNoSql
{
    public class CustomerDataNoSql
    {
        private readonly IMongoCollection<Customer> _collection;

        public CustomerDataNoSql(IOptions<SalesDatabaseSetting> salesDatabaseSetting)
        {
            var mongoClient = new MongoClient(salesDatabaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(salesDatabaseSetting.Value.DatabaseName);
            _collection = mongoDatabase.GetCollection<Customer>(salesDatabaseSetting.Value.CustomerCollection);
        }

        public async Task<List<Customer>> GetCustomersAsync() => await _collection.Find(x => true).ToListAsync();

        public async Task<Customer?> GetCustomerAsync(string id) => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        
        public async Task CreateCustomerAsync(Customer customer) => await _collection.InsertOneAsync(customer);

        public async Task<Customer> UpdateCustomerAsync(string Id, Customer Customer)
        {
            var cust = await _collection.Find(x=>x.Id == Id).FirstOrDefaultAsync();
            if(cust == null)
                return null;

            await _collection.ReplaceOneAsync(x=>x.Id == Id, Customer);

            return Customer;
        }

        public async Task DeleteCustomerAsync(string Id)
        {
            await _collection.DeleteOneAsync(x=>x.Id == Id);
        }
    }
}