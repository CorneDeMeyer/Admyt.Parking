namespace Parking.Domain.Models
{
    public class GateEventResponse<T>
    {
        public List<string> Errors { get; set; }
        public T Value { get; set; }

        public GateEventResponse()
        {
            Errors = new List<string>();
        }
    }
}
