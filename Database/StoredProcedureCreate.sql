CREATE OR ALTER PROCEDURE [dbo].[CreateGate]
	@Name VARCHAR(255),
	@Type int,
	@ZoneId UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO dbo.[Gate]([Name], [Type], [ZoneId])
	VALUES (@Name, @Type, @ZoneId)
	
	SELECT [Id] FROM dbo.[Gate] WHERE [Name] = @Name

END
GO

CREATE OR ALTER PROC dbo.[CreateGateEvent]
	@GateId UNIQUEIDENTIFIER,
	@TimeStamp DATETIME2,
	@PlateText VARCHAR(100),
	@ParkingSessionId UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO dbo.[GateEvent]([GateId], [TimeStamp], [PlateText], [ParkingSessionId])
	VALUES (@GateId, @TimeStamp, @PlateText, @ParkingSessionId)

	SELECT [Id] FROM dbo.[GateEvent] WHERE [ParkingSessionId] = @ParkingSessionId AND [PlateText] = @PlateText

END
GO

CREATE OR ALTER PROC dbo.[CreateParkingSession]
	@PlateText VARCHAR(100),
	@Status INT
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO dbo.[ParkingSession]([PlateText], [Status])
	VALUES(@PlateText, @Status)

	SELECT [Id] FROM dbo.[ParkingSession] WHERE [PlateText] = @PlateText AND [Status] = 1

END
GO

CREATE OR ALTER PROC dbo.[CreateZone]
	@Name VARCHAR(155),
	@ParentZone UNIQUEIDENTIFIER = NULL,
	@Depth INT,
	@Rate NUMERIC(18, 8)
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO dbo.[Zone]([Name], [ParentZone], [Depth], [Rate])
	VALUES(@Name, @ParentZone, @Depth, @Rate)

	SELECT [Id] FROM dbo.[Zone] WHERE [Name] = @Name

END
GO

USE [Parking]
GO

CREATE OR ALTER PROC dbo.[GetAllGate]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [Type],
		   [ZoneId]
	  FROM dbo.[Gate]

END
GO

CREATE OR ALTER PROC dbo.[GetByGateIdGateEvent]
	@GateId UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;

	SELECT [Id],
		   [GateId],
		   [TimeStamp],
		   [PlateText],
		   [ParkingSessionId]
	  FROM dbo.[GateEvent]
	 WHERE [GateId] = @GateId

END
GO

CREATE OR ALTER PROC dbo.[GetByIdGate]
	@Id UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;

	SELECT [Id],
		   [Name],
		   [Type],
		   [ZoneId]
	  FROM dbo.[Gate]
	 WHERE [Id] = @Id

END
GO

CREATE OR ALTER PROC dbo.[GetByIdZone]
	@Id UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;

	SELECT [Id]
		  ,[ParentZone]
		  ,[Depth]
		  ,[Rate]
	  FROM dbo.[Zone]
	 WHERE [Id] = @Id

END
GO

CREATE OR ALTER PROC dbo.[GetByPlateIdGateEvent]
	@PlateText VARCHAR(100)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT [Id],
		   [GateId],
		   [TimeStamp],
		   [PlateText],
		   [ParkingSessionId]
	  FROM dbo.[GateEvent]
	 WHERE [PlateText] = @PlateText

END
GO

CREATE OR ALTER PROC dbo.[GetLatestByPlateIdGateEvent]
	@PlateText VARCHAR(100)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT TOP (1)
		   [Id],
		   [GateId],
		   [TimeStamp],
		   [PlateText],
		   [ParkingSessionId]
	  FROM dbo.[GateEvent]
	 WHERE [PlateText] = @PlateText
  ORDER BY [TimeStamp] DESC

END
GO

CREATE OR ALTER PROC dbo.[GetParkingSessionByPlateText]
	@PlateText VARCHAR(100)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT TOP (1) 
		   ps.[Id], 
		   ps.[PlateText],
		   ps.[Status]
	  FROM dbo.[ParkingSession] ps
	  JOIN dbo.[GateEvent] ge
	    ON ge.[ParkingSessionId] = ps.[Id]
	 WHERE ps.[PlateText] = @PlateText
	   AND EXISTS (SELECT 1 
					 FROM dbo.[GateEvent] ge
					WHERE ge.[ParkingSessionId] = ps.[Id]
					  AND ge.[PlateText] = @PlateText)
  ORDER BY ge.[TimeStamp] DESC
   
END
GO

CREATE OR ALTER PROC dbo.[UpdateGate]
	@Id UNIQUEIDENTIFIER,
	@Name VARCHAR(255),
	@Type INT,
	@ZoneId UNIQUEIDENTIFIER
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE dbo.[Gate]
	   SET [Name] = @Name,
		   [Type] = @Type,
		   [ZoneId] = @ZoneId
	 WHERE [Id] = @Id

END
GO

CREATE OR ALTER PROC dbo.[UpdateParkingSession]
	@Status INT,
	@Id UNIQUEIDENTIFIER
AS
BEGIN

	UPDATE dbo.[ParkingSession]
	   SET [Status] = @Status
	 WHERE [Id] = @Id

END
GO

CREATE OR ALTER PROC dbo.[GetGateEventsByParkingSession]
	@ParkingSessionId UNIQUEIDENTIFIER
AS
BEGIN 
	
	SET NOCOUNT ON;

	SELECT ge.[Id]
		  ,ge.[GateId]
		  ,ge.[TimeStamp]
		  ,ge.[PlateText]
		  ,ge.[ParkingSessionId]
	  FROM [dbo].[GateEvent] ge
	 WHERE ge.[ParkingSessionId] = @ParkingSessionId

END
GO