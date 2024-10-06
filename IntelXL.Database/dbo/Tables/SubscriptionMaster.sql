CREATE TABLE [dbo].[SubscriptionMaster] (
    [SubscriptionId]       INT             IDENTITY (1, 1) NOT NULL,
    [SubscriptionName]     VARCHAR (50)    NULL,
    [SubscriptionDetails]  VARCHAR (50)    NULL,
    [SubscriptionAmount]   DECIMAL (18, 2) NOT NULL,
    [CurrencyType]         VARCHAR(20)     NULL,
    [SubscriptionDuration] INT             NULL,
    [Coupon]               NVARCHAR (25)   NULL, 
    [Status]               BIT             NOT NULL,
    [CourseId]             INT             NOT NULL,
    [ClassId]              INT             NOT NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([SubscriptionId] ASC),
    CONSTRAINT [FK_SubscriptionMaster_CourseMaster] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[CourseMaster] ([CourseID] ),
    CONSTRAINT [FK_SubscriptionMaster_ClassMaster] FOREIGN KEY ([ClassId]) REFERENCES [dbo].[ClassMaster] ([ClassId] ),
     FOREIGN KEY ([CreatedBy] ) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])
);



