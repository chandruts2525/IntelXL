CREATE TABLE [dbo].[TopicMaster] (
    [TopicId] INT            IDENTITY (1, 1) NOT NULL,
    [Topic]   NVARCHAR (200) NOT NULL,
    [UnitId]  INT            NULL,
    [Status]  BIT            NOT NULL,
    [Order]          INT           NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,
    CONSTRAINT [PK_TopicMaster] PRIMARY KEY CLUSTERED ([TopicId] ASC),
    CONSTRAINT [FK_TopicMaster_UnitMaster] FOREIGN KEY ([UnitId]) REFERENCES [dbo].[UnitMaster] ([UnitId]),
     FOREIGN KEY ([CreatedBy] ) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])
);









