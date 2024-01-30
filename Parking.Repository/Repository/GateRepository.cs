using Parking.Domain.Models;
using Parking.Repository.Interface;
using Parking.Repository.Repository.Base;

namespace Parking.Repository.Repository
{
    public class GateRepository : CrudBase, IGateRepository
    {
        public GateRepository(RepositoryConfiguration config) : base(config) { }

        public Task<Guid> Create(Gate gate) => CreateAsync(StoredProcedure.Gate.Create, 
            new Dictionary<string, object>
            {
                { "@Name", gate.Name },
                { "@Type", (int)gate.Type },
                { "@ZoneId", gate.ZoneId }
            });

        public Task<IEnumerable<Gate>> GetAll() =>
            ExecuteQueriesAsync<Gate>(StoredProcedure.Gate.GetAll, new Dictionary<string, object>());

        public Task<Gate> GetById(Guid id) =>
            ExecuteQueryAsync<Gate>(StoredProcedure.Gate.GetById, new Dictionary<string, object>
            {
                { "@Id", id }
            });

        public Task Update(Gate gate) => ExecuteNonQueryAsync(StoredProcedure.Gate.Update,
            new Dictionary<string, object>
            {
                { "@Id", gate.Id },
                { "@Name", gate.Name },
                { "@Type", gate.Type },
                { "@ZoneId", gate.ZoneId },
            });
    }
}
