using Parking.Domain.Models;

namespace Parking.Repository.Interface
{
    public interface IParkingSessionRepository
    {
        Task<Guid> Create(ParkingSession parkingSession);
        Task<ParkingSession> GetByPlate(string plateText);
        Task Update(ParkingSession parkingSession);
    }
}
