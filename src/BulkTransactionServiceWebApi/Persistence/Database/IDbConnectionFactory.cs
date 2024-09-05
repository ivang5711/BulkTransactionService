using System.Data;

namespace BulkTransactionServiceWebApi.Persistence.Database;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}