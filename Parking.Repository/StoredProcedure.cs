namespace Parking.Repository
{
    internal static class StoredProcedure
    {
        public static class Gate
        {
            public const string GetAll = "dbo.[GetAllGate]";
            public const string Create = "dbo.[CreateGate]";
            public const string GetById = "dbo.[GetByIdGate]";
            public const string Update = "dbo.[UpdateGate]";
        }

        public static class GateEvent
        {
            public const string Create = "dbo.[CreateGateEvent]";
            public const string GetByGateId = "dbo.[GetByGateIdGateEvent]";
            public const string GetByPlateId = "dbo.[GetByPlateIdGateEvent]";
            public const string GetLatestByPlateId = "dbo.[GetLatestByPlateIdGateEvent]";
            public const string GetByParkingSessionId = "dbo.[GetGateEventsByParkingSession]";
        }

        public static class ParkingSession
        {
            public const string Create = "dbo.[CreateParkingSession]";
            public const string Update = "dbo.[UpdateParkingSession]";
            public const string GetByPlate = "dbo.[GetParkingSessionByPlateText]";
        }

        public static class Zone 
        {
            public const string GetAll = "dbo.[GetAllZone]";
            public const string Create = "dbo.[CreateZone]";
            public const string Update = "dbo.[UpdateZone]";
            public const string GetById = "dbo.[GetByIdZone]";
            public const string GetByName = "dbo.[GetByNameZone]";
        }
    }
}
