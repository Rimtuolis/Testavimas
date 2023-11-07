namespace PSA.Services
{
    public interface IDatabaseOperationsService
    {
        Task ExecuteAsync(string query);

        Task<List<T>> ReadListAsync<T>(string query);

        Task<T?> ReadItemAsync<T>(string query);
    }
}