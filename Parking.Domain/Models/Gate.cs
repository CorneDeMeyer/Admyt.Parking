using Parking.Domain.Enum;

namespace Parking.Domain.Models
{
    /// <summary>
    /// Defines a gate belonging to a zone, can be either an entry or exit gate
    /// </summary>
    public class Gate
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the gate
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Type of the gate, either Entry or Exit 
        /// </summary>
        public GateType Type { get; set; }

        /// <summary>
        /// The zone the gate belongs to
        /// </summary>
        public Guid ZoneId { get; set; }
    }
}
