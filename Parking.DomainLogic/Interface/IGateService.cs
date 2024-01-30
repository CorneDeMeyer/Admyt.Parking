using Parking.Domain.Models;

namespace Parking.DomainLogic.Interface
{
    public interface IGateService
    {
        Task<Guid> ProcessEntryRequest(GateEvent gateEvent);
        Task<double> ProcessExitResponse(GateEvent gateEvent);
    }
}
