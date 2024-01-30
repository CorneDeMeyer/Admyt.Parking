using Parking.Domain.Models;
using Parking.Repository.Interface;
using Parking.Repository.Repository.Base;

namespace Parking.Repository.Repository
{
    public class ParkingSessionRepository : CrudBase, IParkingSessionRepository
    {
        public ParkingSessionRepository(RepositoryConfiguration config) : base(config) {}

        public Task<Guid> Create(ParkingSession parkingSession) =>
            CreateAsync(StoredProcedure.ParkingSession.Create,
                new Dictionary<string, object>()
                {
                    { "@PlateText", parkingSession.PlateText },
                    { "@Status", (int)parkingSession.Status }
                });

        public Task<ParkingSession> GetByPlate(string plateText) => 
            ExecuteQueryAsync<ParkingSession>(StoredProcedure.ParkingSession.GetByPlate,
                new Dictionary<string, object>()
                {
                    { "@PlateText", plateText }
                });

        public Task Update(ParkingSession parkingSession)
            => ExecuteNonQueryAsync(StoredProcedure.ParkingSession.Update, new Dictionary<string, object>()
                {
                    { "@Id", parkingSession.Id },
                    { "@Status", (int)parkingSession.Status}
                });
    }
}
