CREATE TABLE [dbo].[AppUser] (
    [AppUserId]   INT            IDENTITY (1, 1) NOT NULL,
    [UserName]    VARCHAR (50)   NOT NULL,
    [Password]    NVARCHAR (100) NULL,
    [AppRoleId]   INT            NULL,
    [FirstName]   VARCHAR (50)   NOT NULL,
    [LastName]    VARCHAR (50)   NOT NULL,
    [EmailId]     VARCHAR (50)   NOT NULL,   
    [UpdatedDttm] DATETIME       NULL,    
    [UpdatedBy]   INT       NULL,
    [CreatedDttm] DATETIME       NULL,  
    [Status]      BIT            NOT NULL,
    [IsVerified]  BIT            NULL,
    CONSTRAINT [PK_AppUser] PRIMARY KEY CLUSTERED ([AppUserId] ASC),
    FOREIGN KEY ([AppRoleId]) REFERENCES [dbo].[AppRole] ([AppRoleID])
);







