using Parking.Domain.Models;
using Parking.Repository.Interface;
using Parking.Repository.Repository.Base;

namespace Parking.Repository.Repository
{
    public class GateEventRepository : CrudBase, IGateEventRepository
    {
        public GateEventRepository(RepositoryConfiguration config) : base(config) { }

        public Task<Guid> Create(GateEvent gateEvent) =>
            CreateAsync(StoredProcedure.GateEvent.Create, 
                new Dictionary<string, object>
                {
                    { "@GateId", gateEvent.GateId },
                    { "@Timestamp", gateEvent.TimeStamp },
                    { "@PlateText", gateEvent.PlateText },
                    { "@ParkingSessionId", gateEvent.ParkingSessionId },
                });

        public Task<IEnumerable<GateEvent>> GetByGateId(Guid gateId) =>
            ExecuteQueriesAsync<GateEvent>(StoredProcedure.GateEvent.GetByGateId,
                new Dictionary<string, object>
                {
                    { "@GateId", gateId},
                });

        public Task<IEnumerable<GateEvent>> GetByParkingSessionId(Guid parkingSessionId) =>
            ExecuteQueriesAsync<GateEvent>(StoredProcedure.GateEvent.GetByParkingSessionId,
                new Dictionary<string, object>
                {
                    { "@ParkingSessionId", parkingSessionId }
                });

        public Task<IEnumerable<GateEvent>> GetByPlate(string plateText) =>
            ExecuteQueriesAsync<GateEvent>(StoredProcedure.GateEvent.GetByPlateId,
                new Dictionary<string, object>
                {
                    { "@PlateText", plateText }
                });

        public Task<GateEvent> GetLatestByPlate(string plateText) =>
            ExecuteQueryAsync<GateEvent>(StoredProcedure.GateEvent.GetLatestByPlateId,
                new Dictionary<string, object>
                {
                    { "@PlateText", plateText }
                });
    }
}
