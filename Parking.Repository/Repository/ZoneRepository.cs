using Parking.Domain.Models;
using Parking.Repository.Interface;
using Parking.Repository.Repository.Base;

namespace Parking.Repository.Repository
{
    public class ZoneRepository : CrudBase, IZoneRepository
    {
        public ZoneRepository(RepositoryConfiguration config) : base(config)
        {
        }

        public Task<Guid> Create(Zone zone) =>
            CreateAsync(StoredProcedure.Zone.Create,
                new Dictionary<string, object>
                {
                    { "@Name", zone.Name },
                    { "@ParentZone", zone.ParentZoneId.HasValue ? zone.ParentZoneId.Value : DBNull.Value },
                    { "@Depth", zone.Depth },
                    { "@Rate", zone.Rate },
                });

        public Task<IEnumerable<Zone>> GetAll() =>
            ExecuteQueriesAsync<Zone>(StoredProcedure.Zone.GetAll,
                new Dictionary<string, object>());

        public Task<Zone> GetById(Guid zoneId) =>
            ExecuteQueryAsync<Zone>(StoredProcedure.Zone.GetById,
                new Dictionary<string, object>
                {
                    { "@Id", zoneId }
                });

        public Task<Zone> GetByName(string name) =>
            ExecuteQueryAsync<Zone>(StoredProcedure.Zone.GetByName,
                new Dictionary<string, object>
                {
                    { "@Name", name }
                });

        public Task Update(Zone zone) =>
            ExecuteNonQueryAsync(StoredProcedure.Zone.Update,
                new Dictionary<string, object>
                {
                    { "@Id", zone.Id },
                    { "@Name", zone.Name },
                    { "@ParentZone", zone.ParentZoneId.HasValue ? zone.ParentZoneId.Value : DBNull.Value },
                    { "@Depth", zone.Depth },
                    { "@Rate", zone.Rate },
                });
    }
}
