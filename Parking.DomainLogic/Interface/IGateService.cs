using Parking.Domain.Models;

namespace Parking.DomainLogic.Interface
{
    public interface IGateService
    {
        Task<GateEventResponse<Guid>> ProcessGateRequest(GateEvent gateEvent);
        Task<GateEventResponse<double>> CalculateFee(string plateText);
    }
}
