using Parking.Domain.Models;

namespace Parking.Repository.Interface
{
    public interface IZoneRepository
    {
        public Task<Guid> Create(Zone zone);
        public Task Update(Zone zone);
        public Task<Zone> GetById(Guid zoneId);
        public Task<Zone> GetByName(string name);
        public Task<IEnumerable<Zone>> GetAll();
    }
}
