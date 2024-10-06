CREATE TABLE [dbo].[UserLogin] (
    [UserLoginId]     INT            IDENTITY (1, 1) NOT NULL,
    [UserId]          INT            NOT NULL,
    [LoginDttm]       DATETIME       NOT NULL,
    [LogoutDttm]      DATETIME       NULL,
    [DeviceIpAddress] NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserLoginId] ASC),
    CONSTRAINT [FK_UserLogin_AppUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AppUser] ([AppUserId]),
    CONSTRAINT [UC_UserLogin_UserId] UNIQUE NONCLUSTERED ([UserId] ASC)
);



