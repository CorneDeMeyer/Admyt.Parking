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

        public async Task<Guid> CreateAsync(string storedProcedure, Dictionary<string, object> parameters)
        {
            using (var connection = new SqlConnection(repositoryConfiguration.ConnectionString))
            {
                await connection.OpenAsync();

                var result = await connection.QueryFirstOrDefaultAsync<Guid>(storedProcedure, parameters);

                await connection.CloseAsync();

                return result;
            }
        }

        public async Task ExecuteNonQueryAsync(string storedProcedure, Dictionary<string, object> parameters) 
        {
            using (var connection = new SqlConnection(repositoryConfiguration.ConnectionString))
            {
                await connection.OpenAsync();

                await connection.ExecuteAsync(storedProcedure, parameters);

                await connection.CloseAsync();
            }
        }

        public async Task<T> ExecuteQueryAsync<T>(string storedProcedure, Dictionary<string, object> parameters)
        {
            using (var connection = new SqlConnection(repositoryConfiguration.ConnectionString))
            {
                await connection.OpenAsync();

                var result = await connection.QueryFirstAsync<T>(storedProcedure, parameters);

                await connection.CloseAsync();

                return result;
            }
            
        }

        public async Task<IEnumerable<T>> ExecuteQueriesAsync<T>(string storedProcedure, Dictionary<string, object> parameters)
        {
            using (var connection = new SqlConnection(repositoryConfiguration.ConnectionString))
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<T>(storedProcedure, parameters); 

                await connection.CloseAsync();

                return result;
            }
        }
    }
}
