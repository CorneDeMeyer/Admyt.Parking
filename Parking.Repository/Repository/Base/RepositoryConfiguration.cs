namespace Parking.Repository.Repository.Base
{
    public class RepositoryConfiguration(string connectionString)
    {
        public string ConnectionString { get; private set; } = connectionString;
    }
}
