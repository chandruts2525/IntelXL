CREATE TABLE [dbo].[TutorEducation] (
    [EducationId]            INT            IDENTITY (1, 1) NOT NULL,
    [University]             NVARCHAR (MAX) NOT NULL,
    [Degree]                 NVARCHAR (100) NOT NULL,
    [Specialization]         NVARCHAR (100) NOT NULL,
    [Years_of_study]         VARCHAR (25)   NOT NULL,
    [CertificateUrl]         NVARCHAR (MAX) NOT NULL,
    [AppUserId]              INT            NOT NULL,   
    [CreatedDttm] DATETIME       NULL,
    [UpdatedDttm] DATETIME       NULL,
    CONSTRAINT [PK_TutorEducation] PRIMARY KEY CLUSTERED ([EducationId] ASC),
    CONSTRAINT [FK_TutorEducation_Tutor] FOREIGN KEY ([AppUserId]) REFERENCES [dbo].[AppUser] ([AppUserId]),
);

