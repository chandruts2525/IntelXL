CREATE TABLE [dbo].[AnnimationMaster] (
    [AnnimationId]  INT           IDENTITY (1, 1) NOT NULL,
    [AnnimationUrl] VARCHAR (500) NULL,
    [VideoUrl]      VARCHAR (500) NOT NULL,
    [QuestionId]    INT           NOT NULL,
    [Status]        BIT           NOT NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,    
    CONSTRAINT [PK_AnnimationMaster] PRIMARY KEY CLUSTERED ([AnnimationId] ASC),
    CONSTRAINT [FK_AnnimationMaster_QuestionMaster] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[QuestionMaster] ([QuestionId]),
     FOREIGN KEY ([CreatedBy] ) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])
);







