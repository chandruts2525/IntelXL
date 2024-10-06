CREATE TABLE [dbo].[TutorCertification] (
    [CertificateId]         INT            IDENTITY (1, 1) NOT NULL,
    [Subject]               NVARCHAR (MAX) NOT NULL,
    [CertificateName]       NVARCHAR (100) NOT NULL,
    [Description]           NVARCHAR (100) NOT NULL,
    [Issued_by]             NVARCHAR (100) NOT NULL,
    [Years_of_study]        VARCHAR (25)   NOT NULL,
    [CertificateUrl]        NVARCHAR (MAX) NOT NULL,
    [AppUserId]             INT            NOT NULL,
    [CreatedDttm] DATETIME       NULL,      
    [UpdatedDttm] DATETIME       NULL,
    CONSTRAINT [PK_TutorCertification] PRIMARY KEY CLUSTERED ([CertificateId] ASC),
    CONSTRAINT [FK_TutorCertification_TutorDetails] FOREIGN KEY ([AppUserId]) REFERENCES [dbo].[AppUser] ([AppUserId]),
);

