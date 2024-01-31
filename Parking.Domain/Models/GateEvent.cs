namespace Parking.Domain.Models
{
    /// <summary>
    /// Defines a gate event, either a vehicle entering or exiting a gate at a specific time/// 
    /// </summary>
    public class GateEvent
    {
        /// <summary>
        /// Event Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gate Id
        /// </summary>
        public required Guid GateId { get; set; }

        /// <summary>
        /// Time of the event
        /// </summary>
        public required DateTime TimeStamp { get; set; }

        /// <summary>
        /// Plate text of the vehicle as read by LPR
        /// </summary>
        public required string PlateText { get; set; }

        /// <summary>
        /// Parking session Id the gate event belongs to
        /// </summary>
        public Guid ParkingSessionId { get; set; }
    }
}
