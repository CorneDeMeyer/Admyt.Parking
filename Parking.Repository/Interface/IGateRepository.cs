using Parking.Domain.Models;

namespace Parking.Repository.Interface
{
    public interface IGateRepository
    {
        Task<Guid> Create(Gate gate);
        Task Update(Gate gate);
        Task<Gate> GetById(Guid id);
        Task<IEnumerable<Gate>> GetAll();
    }
}
