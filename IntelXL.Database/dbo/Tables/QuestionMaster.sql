CREATE TABLE [dbo].[QuestionMaster] (
    [QuestionId] INT             IDENTITY (1, 1) NOT NULL,
    [Question]   NVARCHAR (MAX) NOT NULL,
    [SubTopicId] INT             NOT NULL,
    [AnswerId]   INT             NULL,
    [Status]     BIT             NOT NULL,
    [IsVerified] BIT             DEFAULT ((0)) NOT NULL,
    [QuestionType] INT          NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,
    [IsPreviousYearQuestion]     BIT       NULL,
    [PreviousYear]      VARCHAR(100)  NULL,
    [IsProbable]        BIT          NULL,
    [ProbableYear]      VARCHAR(100)       NULL,    
    CONSTRAINT [PK_QuestionMaster] PRIMARY KEY CLUSTERED ([QuestionId] ASC),
    CONSTRAINT [FK_QuestionMaster_AnswerMaster] FOREIGN KEY ([AnswerId]) REFERENCES [dbo].[AnswerMaster] ([AnswerId]),
    CONSTRAINT [FK_QuestionMaster_SubTopicMaster] FOREIGN KEY ([SubTopicId]) REFERENCES [dbo].[SubTopicMaster] ([SubTopicId]),
     FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])
);











