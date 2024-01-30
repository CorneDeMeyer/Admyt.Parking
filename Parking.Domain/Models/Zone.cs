namespace Parking.Domain.Models
{
    /// <summary>
    /// Defines a parking zone
    /// Zones can be nested to create a nested hierarchy
    /// </summary>
    public class Zone
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the zone
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Optional ParentZone if this zone is nested
        /// </summary>
        public Guid? ParentZoneId { get; set; }

        /// <summary>
        /// Indicates the depth of the zone in the hierarchy where 0 is the top level
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// Indicates the cost per hour for parking in the zone
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// Collection of gates in the zone
        /// </summary>
        public ICollection<Gate> Gates => new List<Gate>();
    }
}
