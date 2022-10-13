namespace Sales.API.DataAccess
{
    public interface IDataAccess<T>
    {
        Task<T> GetAsync(string id);
        Task<List<T>> GetManyAsync();
        Task CreateAsync(T obj);
        Task<T> UpdateAsync(string id, T obj);
        Task DeleteAsync(string id);
    }
}