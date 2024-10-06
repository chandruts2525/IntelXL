CREATE TABLE [dbo].[TutorTimingConfig] (
    [TutorTimingId] INT IDENTITY (1, 1) NOT NULL,
    [FromTimeId]    INT NOT NULL,
    [ToTimeId]      INT NOT NULL,
    [TutorId]       INT NOT NULL,
    [DayId]         INT NOT NULL,
    [CreatedDttm] DATETIME       NULL, 
    [UpdatedDttm] DATETIME       NULL,
    CONSTRAINT [PK_TutorTimingConfig] PRIMARY KEY CLUSTERED ([TutorTimingId] ASC),
    CONSTRAINT [FK_TutorTimingConfig_DayMaster] FOREIGN KEY ([DayId]) REFERENCES [dbo].[DayMaster] ([DayId]),
    CONSTRAINT [FK_TutorTimingConfig_FromTimingMaster] FOREIGN KEY ([FromTimeId]) REFERENCES [dbo].[TimingMaster] ([TimingId]),
    CONSTRAINT [FK_TutorTimingConfig_ToTimingMaster] FOREIGN KEY ([ToTimeId]) REFERENCES [dbo].[TimingMaster] ([TimingId]),
    CONSTRAINT [FK_TutorTimingConfig_Tutor] FOREIGN KEY ([TutorId]) REFERENCES [dbo].[AppUser] ([AppUserId])
);


