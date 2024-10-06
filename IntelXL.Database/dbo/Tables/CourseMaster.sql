CREATE TABLE [dbo].[CourseMaster] (
    [CourseID]       INT           IDENTITY (1, 1) NOT NULL,
    [CourseName]     NVARCHAR (50) NULL,
    [CourseDuration] DECIMAL (18)  NULL,    
    [Status]         BIT           NOT NULL,
    [Language_Id]    INT           NULL,
    [Order]          INT           NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([CourseID] ASC),
     FOREIGN KEY ([Language_Id] ) REFERENCES [dbo].[LanguageOfInstructionMaster] ([Language_Id]),
     FOREIGN KEY ([CreatedBy] ) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])
);









