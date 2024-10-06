CREATE TABLE [dbo].[PaymentTypeMaster] (
    [PaymentId]        INT          IDENTITY (1, 1) NOT NULL,
    [PaymentType]      VARCHAR (50) NULL,   
    [AccountNumber]    VARCHAR (20) NULL,
    [Status]           BIT          NOT NULL,
    [CreatedBy]   INT            NULL,
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,
    CONSTRAINT [PK_PaymentTypeMaster] PRIMARY KEY CLUSTERED ([PaymentId] ASC), 
     FOREIGN KEY ([CreatedBy] ) REFERENCES [dbo].[AppUser] ([AppUserId]),
     FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[AppUser] ([AppUserId])
);





