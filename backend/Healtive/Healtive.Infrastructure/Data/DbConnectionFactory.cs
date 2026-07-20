using System.Data;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Healtive.Infrastructure.Data;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        var connectionString =
            _configuration.GetConnectionString("DefaultConnection");

        return new MySqlConnection(connectionString);
    }
}