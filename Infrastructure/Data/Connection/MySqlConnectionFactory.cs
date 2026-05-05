using MySqlConnector;
using Microsoft.Extensions.Configuration;
namespace Infrastructure.Data.Connection
{
    public class MySqlConnectionFactory
    {
        private readonly string _connectionString;

        public MySqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")
                ?? throw new Exception("Connection string 'Default' não encontrada.");
        }

        public MySqlConnection Create()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
