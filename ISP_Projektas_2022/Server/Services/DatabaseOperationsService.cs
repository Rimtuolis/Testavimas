using Dapper;
using MySqlConnector;

namespace ISP_Projektas_2022.Services
{
    public class DatabaseOperationsService : IDatabaseOperationsService, IDisposable
    {
        private readonly MySqlConnection _connection;

        public DatabaseOperationsService(MySqlConnection connection)
        {
            _connection = connection;
            _connection.OpenAsync();
        }

        public async Task<List<T>> ReadListAsync<T>(string query)
        {
            if (!query.Contains("select", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidOperationException("Read endpoint used for non-read operation");
            }
            List<T> itemList = new List<T>();
            using var command = new MySqlCommand(query, _connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var parser = reader.GetRowParser<T>(typeof(T));
                var item = parser(reader);
                itemList.Add(item);
            }

            return itemList;
        }

        public async Task<T?> ReadItemAsync<T>(string query)
        {
            if (!query.Contains("select", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidOperationException("Read endpoint used for non-read operation");
            }

            using var command = new MySqlCommand(query, _connection);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var parser = reader.GetRowParser<T?>(typeof(T));
                var item = parser(reader);
                return item;
            }

            return default;
        }

        public async Task ExecuteAsync(string query)
        {
            using var command = new MySqlCommand(query, _connection);
            await command.ExecuteNonQueryAsync();
        }

        public async void Dispose()
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }
    }
}