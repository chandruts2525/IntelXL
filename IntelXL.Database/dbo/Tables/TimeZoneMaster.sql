CREATE TABLE [dbo].[TimeZoneMaster] (
    [TimezoneId]   INT           IDENTITY (1, 1) NOT NULL,
    [TimezoneName] VARCHAR (MAX) NULL,
    [UtcOffSet]    VARCHAR (10)  NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,
    CONSTRAINT [PK_TimeZoneMaster] PRIMARY KEY CLUSTERED ([TimezoneId] ASC),
     FOREIGN KEY ([CreatedBy] ) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])
);

