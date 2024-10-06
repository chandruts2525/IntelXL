CREATE TABLE [dbo].[DayMaster] (
    [DayId] INT          IDENTITY (1, 1) NOT NULL,
    [DayName]   VARCHAR (15) NOT NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,
    CONSTRAINT [PK_DayMaster] PRIMARY KEY CLUSTERED ([DayId] ASC),
     FOREIGN KEY ([CreatedBy] ) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])
);

