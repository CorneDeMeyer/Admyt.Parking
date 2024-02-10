using Parking.Domain.Models;

namespace Parking.Repository.Interface
{
    public interface IGateEventRepository
    {
        public Task<Guid> Create(GateEvent gateEvent);
        public Task<GateEvent> GetLatestByPlate(string plateText);
        public Task<IEnumerable<GateEvent>> GetByPlate(string plateText);
        public Task<IEnumerable<GateEvent>> GetByGateId(Guid gateId);
        public Task<IEnumerable<GateEvent>> GetByParkingSessionId(Guid parkingSessionId);
    }
}
