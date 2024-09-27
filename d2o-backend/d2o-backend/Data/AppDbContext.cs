using System.Data;
using Microsoft.Data.SqlClient;

namespace d2o_backend.Data
{
    public class AppDbContext
    {
        public IConfiguration _configuration;
        public readonly string _connectionString;

        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
