namespace Parking.Domain.Models
{
    public class GateRequest
    {
        public required Guid GateId { get; set; }
        public required string plateText { get; set; }
        public required DateTime Timestamp { get; set; }
    }
}
