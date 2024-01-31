namespace Parking.Domain.Models
{
    public class GateEventHubModel(GateEvent request, Guid? response, List<string> errors)
    {
        public GateEvent Request { get; private set; } = request;
        public Guid? Response {  get; private set; } = response;
        public List<string> Errors { get; private set; } = errors;

    }
}
