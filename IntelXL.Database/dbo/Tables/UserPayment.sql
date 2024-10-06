CREATE TABLE UserPayment (
    [PaymentID]       INT IDENTITY (1, 1) NOT NULL,
    [AppUserId]       INT NOT NULL,
    [Status]          BIT NOT NULL,
    [AmountPaid]          DECIMAL (18, 2) NOT NULL,
    [InitialAmount]          DECIMAL (18, 2) NOT NULL,
    [PaymentDate]     DATETIME NOT NULL,
    [RecurringMethod] VARCHAR (50) NOT NULL,
    [ResponsePaymentId] NVARCHAR(50) NULL, 
    [FirstName] NVARCHAR(50) NULL, 
    [LastName] NVARCHAR(50) NULL, 
    [Contact] NVARCHAR(50) NULL, 
    [Email] NVARCHAR(50) NULL, 
    [CouponId]         INT NULL,    
    CONSTRAINT [PK_UserPayment] PRIMARY KEY CLUSTERED ([PaymentID] ASC),
    CONSTRAINT [FK_UserPayment_AppUser] FOREIGN KEY ([AppUserId]) REFERENCES [dbo].[AppUser] ([AppUserId]),
    FOREIGN KEY ([CouponId] ) REFERENCES [dbo].[CouponMaster] ([Id]),
);



