CREATE TABLE [dbo].[TimingMaster] (
    [TimingId] INT         IDENTITY (1, 1) NOT NULL,
    [Timing]   VARCHAR (5) NOT NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,
    CONSTRAINT [PK_TimingMaster] PRIMARY KEY CLUSTERED ([TimingId] ASC),
     FOREIGN KEY ([CreatedBy] ) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])
);

