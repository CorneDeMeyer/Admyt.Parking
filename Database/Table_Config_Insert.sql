
BEGIN TRY
	BEGIN TRANSACTION

		DECLARE @ZoneA UNIQUEIDENTIFIER;

		INSERT INTO dbo.[Zone]([Name], [ParentZone], [Depth], [Rate])
		VALUES ('A', NULL, 0, 5.00)

		SELECT @ZoneA = [Id]
		  FROM dbo.[Zone]
		 WHERE [Name] = 'A'

		 DECLARE @ZoneB UNIQUEIDENTIFIER;

		INSERT INTO dbo.[Zone]([Name], [ParentZone], [Depth], [Rate])
		VALUES   ('B', @ZoneA, 1, 10.00)
		
		SELECT @ZoneB = [Id]
		  FROM dbo.[Zone]
		 WHERE [Name] = 'B'

		INSERT INTO dbo.[Gate]([Name], [Type], [ZoneId])
		VALUES ('A1', 1, @ZoneA),
			   ('A2', 1, @ZoneA),
			   ('A3', 2, @ZoneA),
			   ('A4', 2, @ZoneA),
			   ('B1', 1, @ZoneB),
			   ('B2', 2, @ZoneB);

		COMMIT TRANSACTION;

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