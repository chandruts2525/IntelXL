CREATE TABLE [dbo].[UnitMaster] (
    [UnitId]    INT            IDENTITY (1, 1) NOT NULL,
    [UnitName]  NVARCHAR (100) NULL,
    [Subjectid] INT            NOT NULL,
    [Status]    BIT            NOT NULL,
    [Order]          INT           NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,
    CONSTRAINT [PK_UnitMaster] PRIMARY KEY CLUSTERED ([UnitId] ASC),
    CONSTRAINT [FK_UnitMaster_SubjectMaster] FOREIGN KEY ([Subjectid]) REFERENCES [dbo].[SubjectMaster] ([SubjectId]),
     FOREIGN KEY ([CreatedBy] ) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])
);









