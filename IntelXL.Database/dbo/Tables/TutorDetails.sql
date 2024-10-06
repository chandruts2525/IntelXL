CREATE TABLE [dbo].[TutorDetails] (
    [DetailId]       INT                IDENTITY (1, 1) NOT NULL,
    [PhoneNumber]    VARCHAR (20)       NOT NULL,
    [ShortBio]       NVARCHAR (MAX)     NOT NULL,
    [Country]        VARCHAR (50)       NULL,
    [ProfileUrl]     VARCHAR (MAX)      NOT NULL,
    [LanguagesSpeak] VARCHAR (MAX)      NOT NULL,
    [VideoUrl]       VARCHAR (MAX)      NOT NULL,
    [Pricing]        DECIMAL (18, 2)    NOT NULL,
    [TimeZone]       DATETIMEOFFSET (7) NOT NULL,
    [TutorId]        INT                NOT NULL,
    [CourseId]       INT                NULL,
    [ClassId]        INT                NULL,
    [SubjectId]      INT                NULL,   
    [CreatedDttm] DATETIME       NULL,      
    [UpdatedDttm] DATETIME       NULL,
    CONSTRAINT [PK_TutorDetails] PRIMARY KEY CLUSTERED ([DetailId] ASC),
    CONSTRAINT [FK_TutorDetails_AppUser] FOREIGN KEY ([TutorId]) REFERENCES [dbo].[AppUser] ([AppUserId])
);









