CREATE TABLE [dbo].[SubTopicMaster] (
    [SubTopicId] INT            IDENTITY (1, 1) NOT NULL,
    [SubTopic]   NVARCHAR (500) NOT NULL,
    [TopicId]    INT            NOT NULL,
    [Status]     BIT            NOT NULL,
    [Order]          INT           NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,
    CONSTRAINT [PK_SubTopicMaster] PRIMARY KEY CLUSTERED ([SubTopicId] ASC),
    CONSTRAINT [FK_SubTopicMaster_TopicMaster] FOREIGN KEY ([TopicId]) REFERENCES [dbo].[TopicMaster] ([TopicId]),
     FOREIGN KEY ([CreatedBy] ) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])
);









