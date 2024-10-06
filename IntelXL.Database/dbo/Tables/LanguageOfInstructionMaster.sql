CREATE TABLE [dbo].[LanguageOfInstructionMaster] (
    [Language_Id] INT           IDENTITY (1, 1) NOT NULL,
    [Language]    NVARCHAR (50) NOT NULL,
    [Status]      BIT           NOT NULL,
    [Order]          INT           NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Language_Id] ASC),
     FOREIGN KEY ([CreatedBy] ) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])
);

