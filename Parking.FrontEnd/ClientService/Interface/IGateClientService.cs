using Parking.Domain.Models;

namespace Parking.FrontEnd.ClientService.Interface
{
    public interface IGateClientService
    {
        Task<GateEventResponse<Guid>> ProcessGateRequest(GateEvent gateEvent);
        Task<GateEventResponse<double>> CalculateFee(string plateText);
    }
}
