namespace Parking.Domain.Models
{
    public class GateEventResponse<T>
    {
        public List<string> Errors { get; set; } = new();
        public T? Value { get; set; }
    }
}
