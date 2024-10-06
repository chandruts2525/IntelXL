CREATE TABLE [dbo].[SubjectMaster] (
    [SubjectId]   INT            IDENTITY (1, 1) NOT NULL,
    [ClassId]     INT            NOT NULL,
    [SubjectName] NVARCHAR (100) NULL,
    [Status]      BIT            NOT NULL,
    [Order]          INT           NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,
    CONSTRAINT [PK_SubjectMaster] PRIMARY KEY CLUSTERED ([SubjectId] ASC),
    CONSTRAINT [FK_SubjectMaster_ClassMaster] FOREIGN KEY ([ClassId]) REFERENCES [dbo].[ClassMaster] ([ClassId]),
     FOREIGN KEY ([CreatedBy] ) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])
);









