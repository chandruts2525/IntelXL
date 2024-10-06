CREATE TABLE [dbo].[AppRole] (
    [AppRoleID]   INT          IDENTITY (1, 1) NOT NULL,
    [RoleName]    VARCHAR (50) NOT NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,   
    [Status]      BIT          NOT NULL,
    CONSTRAINT [PK_AppRole] PRIMARY KEY CLUSTERED ([AppRoleID] ASC),
     FOREIGN KEY ([CreatedBy] ) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])
);





