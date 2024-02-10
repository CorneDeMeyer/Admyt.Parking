
CREATE TABLE [dbo].[Zone] (
    [Id]         UNIQUEIDENTIFIER NOT NULL DEFAULT(NEWID()),
    [Name]       VARCHAR (155)    NOT NULL,
    [ParentZone] UNIQUEIDENTIFIER NULL,
    [Depth]      INT              NOT NULL,
    [Rate]       NUMERIC (18, 8)  NOT NULL,
	CONSTRAINT [pk_Zone_Id] PRIMARY KEY ([Id]),
	CONSTRAINT [fk_Zone_ParentZone_Id] FOREIGN KEY ([ParentZone]) REFERENCES [Zone]([Id])
);

CREATE TABLE [dbo].[Gate] (
    [Id]     UNIQUEIDENTIFIER NOT NULL DEFAULT(NEWID()),
    [Name]   VARCHAR (155)    NOT NULL,
    [Type]   INT              NOT NULL,
    [ZoneId] UNIQUEIDENTIFIER NOT NULL,
	[IsActive] BIT NOT NULL,
	CONSTRAINT [df_Gate_IsActive] DEFAULT(1),
	CONSTRAINT [pk_Gate_Id] PRIMARY KEY ([Id]),
	CONSTRAINT [fk_Gate_Zoned] FOREIGN KEY ([ZoneId]) REFERENCES [Zone]([Id])
);

CREATE TABLE [dbo].[ParkingSession] (
    [Id]        UNIQUEIDENTIFIER NOT NULL DEFAULT(NEWID()),
    [PlateText] VARCHAR (100)    NOT NULL,
    [Status]    INT              NOT NULL,
	CONSTRAINT [pk_ParkingSession_Id] PRIMARY KEY ([Id]),
);

CREATE TABLE [dbo].[GateEvent] (
    [Id]               UNIQUEIDENTIFIER NOT NULL DEFAULT(NEWID()),
    [GateId]           UNIQUEIDENTIFIER NOT NULL,
    [TimeStamp]        DATETIME2 (7)    NOT NULL,
    [PlateText]        VARCHAR (100)    NOT NULL,
    [ParkingSessionId] UNIQUEIDENTIFIER NOT NULL,
	CONSTRAINT [pk_GateEvent_Id] PRIMARY KEY ([Id]),
	CONSTRAINT [fk_GateEvent_ParkingSessionId] FOREIGN KEY ([ParkingSessionId]) REFERENCES [ParkingSession]([Id]),
	CONSTRAINT [fk_GateEvent_GateId] FOREIGN KEY ([GateId]) REFERENCES [Gate]([Id])
);

ALTER TABLE [Gate]
ADD CONSTRAINT df_Gate_Id
DEFAULT NEWID() FOR Id; 

ALTER TABLE [GateEvent]
ADD CONSTRAINT df_GateEvent_Id
DEFAULT NEWID() FOR Id; 

ALTER TABLE [ParkingSession]
ADD CONSTRAINT df_ParkingSession_Id
DEFAULT NEWID() FOR Id; 

ALTER TABLE [Zone]
ADD CONSTRAINT df_Zone_Id
DEFAULT NEWID() FOR Id;  