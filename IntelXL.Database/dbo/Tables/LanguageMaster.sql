CREATE TABLE [dbo].[LanguageMaster] (
[LanguageId] INT          IDENTITY (1, 1) NOT NULL,
[Language]    NVARCHAR (50) NOT NULL,
[Status]      BIT       NOT NULL,
[CreatedBy]   INT            NULL,
[CreatedDttm] DATETIME       NULL, 
[UpdatedBy]   INT       NULL,
[UpdatedDttm] DATETIME       NULL,
PRIMARY KEY CLUSTERED ([LanguageId] ASC), 
     FOREIGN KEY ([CreatedBy] ) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])   
);

