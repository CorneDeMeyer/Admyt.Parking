using Parking.Domain.Enum;

namespace Parking.Domain.Models
{
    public class ParkingSession
    {
        /// <summary>
        /// Parking session Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Plate text of the vehicle as read by LPR
        /// </summary>
        public required string PlateText { get; set; }

        /// <summary>
        /// Status of the parking session
        /// </summary>
        public SessionStatus Status { get; set; }

        /// <summary>
        /// Gate events belonging to the parking session
        /// </summary>
        public ICollection<GateEvent> GateEvents => new List<GateEvent>();
    }
}
