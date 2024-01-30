namespace Parking.Domain.Enum
{
    /// <summary>
    /// Defines the status of a parking session
    /// </summary>
    public enum SessionStatus
    {
        /// <summary>
        /// Parking session is active, i.e. vehicle has entered but not yet exited
        /// </summary>
        Active = 1,

        /// <summary>
        /// Parking session is completed, i.e. vehicle has entered and exited
        /// </summary>
        Completed = 2,
    }
}
