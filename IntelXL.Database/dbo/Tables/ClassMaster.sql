CREATE TABLE [dbo].[ClassMaster] (
    [ClassId]     INT            IDENTITY (1, 1) NOT NULL,
    [ClassName]   NVARCHAR (100) NOT NULL,
    [CourseId]    INT            NOT NULL,
    [Status]      BIT            NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Order]       INT            NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,
    CONSTRAINT [PK_ClassMaster] PRIMARY KEY CLUSTERED ([ClassId] ASC),
    CONSTRAINT [FK_ClassMaster_CourseMaster] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[CourseMaster] ([CourseID]),
     FOREIGN KEY ([CreatedBy] ) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])
);









