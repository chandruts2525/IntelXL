CREATE TABLE [dbo].[Chats] (
    [ChatId]         INT            IDENTITY (1, 1) NOT NULL,
    [FromId]         INT            NOT NULL,
    [ToId]           INT            NOT NULL,
    [Message]        NVARCHAR (MAX) NULL,
    [FileName]       NVARCHAR (100) NULL,
    [MediaUrl]       VARCHAR (500)  NULL,
    [SentAt]         DATETIME       NOT NULL,
    [IsDelivered]    BIT            NOT NULL,
    [IsRead]         BIT            NULL,
    [IsArchived]     BIT            NULL,
    [ConversationId] VARCHAR (500)  NULL,
    CONSTRAINT [PK_Chats] PRIMARY KEY CLUSTERED ([ChatId] ASC),
    CONSTRAINT [FK_Chats_FromUser] FOREIGN KEY ([FromId]) REFERENCES [dbo].[AppUser] ([AppUserId]),
    CONSTRAINT [FK_Chats_ToUser] FOREIGN KEY ([ToId]) REFERENCES [dbo].[AppUser] ([AppUserId])
);





