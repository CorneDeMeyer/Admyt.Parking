
DECLARE @GateA1 UNIQUEIDENTIFIER = (SELECT TOP (1) [Id] FROM dbo.[Gate] WHERE [Name] = 'A1'),
		@GateA2 UNIQUEIDENTIFIER = (SELECT TOP (1) [Id] FROM dbo.[Gate] WHERE [Name] = 'A2'),
		@GateA3 UNIQUEIDENTIFIER = (SELECT TOP (1) [Id] FROM dbo.[Gate] WHERE [Name] = 'A3'),
		@GateA4 UNIQUEIDENTIFIER = (SELECT TOP (1) [Id] FROM dbo.[Gate] WHERE [Name] = 'A4'),
		@GateB1 UNIQUEIDENTIFIER = (SELECT TOP (1) [Id] FROM dbo.[Gate] WHERE [Name] = 'B1'),
		@GateB2 UNIQUEIDENTIFIER = (SELECT TOP (1) [Id] FROM dbo.[Gate] WHERE [Name] = 'B2');

BEGIN TRY
	BEGIN TRANSACTION
		-- Scenario 1
		DECLARE @Scenario1PlateText VARCHAR(20) = 'ABC123GP',
				@Scenario1ParkingSessionId UNIQUEIDENTIFIER;
		INSERT INTO dbo.[ParkingSession]([PlateText],[Status])
			 VALUES (@Scenario1PlateText, 1)

		SELECT TOP (1) @Scenario1ParkingSessionId = [Id]
		  FROM [dbo].[ParkingSession]
		 WHERE [PlateText] = @Scenario1PlateText

		INSERT INTO [dbo].[GateEvent]([GateId],[TimeStamp],[PlateText],[ParkingSessionId])
			 VALUES (@GateA1, '2024-01-01 10:00:00', @Scenario1PlateText, @Scenario1ParkingSessionId),
					(@GateB1, '2024-01-01 10:30:00', @Scenario1PlateText, @Scenario1ParkingSessionId),
					(@GateB2, '2024-01-01 12:00:00', @Scenario1PlateText, @Scenario1ParkingSessionId),
					(@GateA3, '2024-01-01 13:10:00', @Scenario1PlateText, @Scenario1ParkingSessionId)

		UPDATE dbo.[ParkingSession]
		   SET [Status] = 2
		 WHERE [Id] = @Scenario1ParkingSessionId

		-- Scenario 2
		DECLARE @Scenario2PlateText VARCHAR(20) = 'ABC333GP',
				@Scenario2ParkingSessionId UNIQUEIDENTIFIER;
		INSERT INTO dbo.[ParkingSession]([PlateText],[Status])
			 VALUES(@Scenario2PlateText, 1)

		SELECT TOP (1) @Scenario2ParkingSessionId = [Id]
		  FROM [dbo].[ParkingSession]
		 WHERE [PlateText] = @Scenario2PlateText

		INSERT INTO [dbo].[GateEvent]([GateId],[TimeStamp],[PlateText],[ParkingSessionId])
			 VALUES (@GateA1, '2024-01-01 10:00:00', @Scenario2PlateText, @Scenario2ParkingSessionId),
					(@GateB1, '2024-01-01 10:04:10', @Scenario2PlateText, @Scenario2ParkingSessionId),
					(@GateB2, '2024-01-01 12:00:00', @Scenario2PlateText, @Scenario2ParkingSessionId),
					(@GateA3, '2024-01-01 13:10:00', @Scenario2PlateText, @Scenario2ParkingSessionId);

		UPDATE dbo.[ParkingSession]
		   SET [Status] = 2
		 WHERE [Id] = @Scenario2ParkingSessionId

		-- Scenario 3
		DECLARE @Scenario3PlateText VARCHAR(20) = 'DEF456GP',
				@Scenario3ParkingSessionId UNIQUEIDENTIFIER;
		INSERT INTO dbo.[ParkingSession]([PlateText],[Status])
			 VALUES(@Scenario3PlateText, 1)

		SELECT TOP (1) @Scenario3ParkingSessionId = [Id]
		  FROM [dbo].[ParkingSession]
		 WHERE [PlateText] = @Scenario3PlateText

		INSERT INTO [dbo].[GateEvent]([GateId],[TimeStamp],[PlateText],[ParkingSessionId])
			 VALUES (@GateA1, '2024-01-01 10:00:00', @Scenario3PlateText, @Scenario3ParkingSessionId),
					(@GateB2, '2024-01-01 12:00:00', @Scenario3PlateText, @Scenario3ParkingSessionId),
					(@GateA3, '2024-01-01 13:00:00', @Scenario3PlateText, @Scenario3ParkingSessionId);

		UPDATE dbo.[ParkingSession]
		   SET [Status] = 2
		 WHERE [Id] = @Scenario3ParkingSessionId

		-- Scenario 4
		DECLARE @Scenario4PlateText VARCHAR(20) = 'JKL000GP',
				@Scenario4ParkingSessionId UNIQUEIDENTIFIER;
		INSERT INTO dbo.[ParkingSession]([PlateText],[Status])
			 VALUES(@Scenario4PlateText, 1)

		SELECT TOP (1) @Scenario4ParkingSessionId = [Id]
		  FROM [dbo].[ParkingSession]
		 WHERE [PlateText] = @Scenario4PlateText

		INSERT INTO [dbo].[GateEvent]([GateId],[TimeStamp],[PlateText],[ParkingSessionId])
			 VALUES (@GateA1, '2024-01-01 10:00:00', @Scenario4PlateText, @Scenario4ParkingSessionId),
					(@GateA3, '2024-01-01 11:00:00', @Scenario4PlateText, @Scenario4ParkingSessionId),
					(@GateA4, '2024-01-01 11:04:00', @Scenario4PlateText, @Scenario4ParkingSessionId);

		UPDATE dbo.[ParkingSession]
		   SET [Status] = 2
		 WHERE [Id] = @Scenario4ParkingSessionId

		-- Scenario 5
		DECLARE @Scenario5PlateText VARCHAR(20) = 'MNO111GP',
				@Scenario5ParkingSessionId UNIQUEIDENTIFIER;
		INSERT INTO dbo.[ParkingSession]([PlateText],[Status])
			 VALUES(@Scenario5PlateText, 1)

		SELECT TOP (1) @Scenario5ParkingSessionId = [Id]
		  FROM [dbo].[ParkingSession]
		 WHERE [PlateText] = @Scenario5PlateText

		INSERT INTO [dbo].[GateEvent]([GateId],[TimeStamp],[PlateText],[ParkingSessionId])
			 VALUES (@GateA1, '2024-01-01 10:00:00', @Scenario5PlateText, @Scenario5ParkingSessionId),
					(@GateA3, '2024-01-01 10:10:00', @Scenario5PlateText, @Scenario5ParkingSessionId);

		UPDATE dbo.[ParkingSession]
		   SET [Status] = 2
		 WHERE [Id] = @Scenario5ParkingSessionId

		-- Scenario 6
		DECLARE @Scenario6PlateText VARCHAR(20) = 'PQR222GP',
				@Scenario6ParkingSessionId UNIQUEIDENTIFIER;
		INSERT INTO dbo.[ParkingSession]([PlateText],[Status])
			 VALUES(@Scenario6PlateText, 1)

		SELECT TOP (1) @Scenario6ParkingSessionId = [Id]
		  FROM [dbo].[ParkingSession]
		 WHERE [PlateText] = @Scenario6PlateText

		INSERT INTO [dbo].[GateEvent]([GateId],[TimeStamp],[PlateText],[ParkingSessionId])
			 VALUES (@GateA1, '2024-01-01 10:00:00', @Scenario6PlateText, @Scenario6ParkingSessionId),
					(@GateA2, '2024-01-01 11:00:00', @Scenario6PlateText, @Scenario6ParkingSessionId),
					(@GateA4, '2024-01-01 11:04:00', @Scenario6PlateText, @Scenario6ParkingSessionId);

		UPDATE dbo.[ParkingSession]
		   SET [Status] = 2
		 WHERE [Id] = @Scenario4ParkingSessionId

		-- Scenario 7
		DECLARE @Scenario7PlateText VARCHAR(20) = 'GHI789GP',
				@Scenario7IncorrectPlateText VARCHAR(20) = 'GH1789GP ',
				@Scenario7ParkingSessionId UNIQUEIDENTIFIER;
		INSERT INTO dbo.[ParkingSession]([PlateText],[Status])
			 VALUES(@Scenario7PlateText, 1)

		SELECT TOP (1) @Scenario7ParkingSessionId = [Id]
		  FROM [dbo].[ParkingSession]
		 WHERE [PlateText] = @Scenario7PlateText

		INSERT INTO [dbo].[GateEvent]([GateId],[TimeStamp],[PlateText],[ParkingSessionId])
			 VALUES (@GateA1, '2024-01-01 10:00:00', @Scenario7PlateText, @Scenario7ParkingSessionId),
					(@GateB1, '2024-01-01 11:00:00', @Scenario7PlateText, @Scenario7ParkingSessionId),
					(@GateB2, '2024-01-02 00:01:00', @Scenario7PlateText, @Scenario7ParkingSessionId),
					(@GateA2, '2024-01-02 13:00:00', @Scenario7PlateText, @Scenario7ParkingSessionId);

		UPDATE dbo.[ParkingSession]
		   SET [Status] = 2
		 WHERE [Id] = @Scenario7ParkingSessionId

		COMMIT TRANSACTION

END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION;
	SELECT
		ERROR_NUMBER() AS ErrorNumber,
		ERROR_STATE() AS ErrorState,
		ERROR_SEVERITY() AS ErrorSeverity,
		ERROR_PROCEDURE() AS ErrorProcedure,
		ERROR_LINE() AS ErrorLine,
		ERROR_MESSAGE() AS ErrorMessage;
END CATCH