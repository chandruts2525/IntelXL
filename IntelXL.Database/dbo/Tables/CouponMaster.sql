CREATE TABLE [dbo].[CouponMaster]
(
	[Id]              INT IDENTITY (1, 1) NOT NULL,
	[Name]            NVARCHAR (50)  NULL,
	[CouponCode]      NVARCHAR (50)  NULL,
	[OfferPercentage] INT            NOT NULL,
	[StartDate]       DATETIME       NULL,  
	[EndDate]         DATETIME       NULL,  
	[Status]          BIT            NOT NULL,
	[CreatedBy]   INT            NULL,
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,
	CONSTRAINT [PK_CouponMaster] PRIMARY KEY CLUSTERED ([Id] ASC),
     FOREIGN KEY ([CreatedBy] ) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])
);
