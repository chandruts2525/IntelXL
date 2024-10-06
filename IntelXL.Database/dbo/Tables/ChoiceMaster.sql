CREATE TABLE [dbo].[ChoiceMaster] (
    [ChoiceId]   INT           IDENTITY (1, 1) NOT NULL,
    [Choice]     NVARCHAR (MAX) NULL,
    [Status]     BIT           NULL,
    [QuestionId] INT           NOT NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,
    CONSTRAINT [PK_ChoiceMaster] PRIMARY KEY CLUSTERED ([ChoiceId] ASC),
    CONSTRAINT [FK_ChoiceMaster_QuestionMaster] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[QuestionMaster] ([QuestionId]),
     FOREIGN KEY ([CreatedBy] ) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])
);

