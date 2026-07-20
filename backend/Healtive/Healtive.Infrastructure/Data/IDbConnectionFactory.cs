using System.Data;

namespace Healtive.Infrastructure.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}