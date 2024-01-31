using Parking.Domain.Models;
using Parking.FrontEnd.ClientService.Interface;

namespace Parking.FrontEnd.ClientService
{
    public class GateClientService : IGateClientService
    {
        public GateClientService() { }

        public Task<GateEventResponse<double>> CalculateFee(string plateText)
        {
            throw new NotImplementedException();
        }

        public Task<GateEventResponse<Guid>> ProcessGateRequest(GateEvent gateEvent)
        {
            throw new NotImplementedException();
        }
    }
}
