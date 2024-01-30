using System.Data.SqlClient;
using Dapper;

namespace Parking.Repository.Repository.Base
{
    public abstract class CrudBase
    {
        private readonly RepositoryConfiguration repositoryConfiguration;
        protected CrudBase(RepositoryConfiguration config) 
        {
            this.repositoryConfiguration = config;
        }

        public Task<Guid> CreateAsync(string storedProcedure, Dictionary<string, object> parameters)
        {
            using var connection = new SqlConnection(repositoryConfiguration.ConnectionString);
            return connection.QueryFirstOrDefaultAsync<Guid>(storedProcedure, parameters);
        }

        public Task ExecuteNonQueryAsync(string storedProcedure, Dictionary<string, object> parameters) 
        {
            using var connection = new SqlConnection(repositoryConfiguration.ConnectionString);
            return connection.ExecuteAsync(storedProcedure, parameters);
        }

        public Task<T> ExecuteQueryAsync<T>(string storedProcedure, Dictionary<string, object> parameters)
        {
            using var connection = new SqlConnection(repositoryConfiguration.ConnectionString);
            return connection.QueryFirstAsync<T>(storedProcedure, parameters);
        }

        public Task<IEnumerable<T>> ExecuteQueriesAsync<T>(string storedProcedure, Dictionary<string, object> parameters)
        {
            using var connection = new SqlConnection(repositoryConfiguration.ConnectionString);
            return connection.QueryAsync<T>(storedProcedure, parameters);
        }
    }
}
