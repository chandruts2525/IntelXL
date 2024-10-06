CREATE TABLE [dbo].[UserSubscription] (
    [UserSubscriptionId] INT  IDENTITY (1, 1) NOT NULL,
    [AppUserId]      INT NOT NULL,
    [SubscriptionId] INT NOT NULL,
    [SubscriptionType] VARCHAR(50) NULL,
    [Status]         BIT      NOT NULL,
    [CreatedDttm]    DATETIME NOT NULL,
    [ExpireDttm]     DATETIME NOT NULL,
    [RemaingDays]    INT      NOT NULL,    
      CONSTRAINT [PK_UserSubscription] PRIMARY KEY CLUSTERED ([UserSubscriptionId] ASC),
    FOREIGN KEY ([AppUserId]) REFERENCES [dbo].[AppUser] ([AppUserId]),
    FOREIGN KEY ([SubscriptionId]) REFERENCES [dbo].[SubscriptionMaster] ([SubscriptionId])
);

