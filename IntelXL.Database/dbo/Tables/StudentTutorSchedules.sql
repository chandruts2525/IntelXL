CREATE TABLE [dbo].[StudentTutorSchedules] (
    [ScheduleId]          INT      IDENTITY (1, 1) NOT NULL,
    [AppUserId]           INT      NOT NULL,
    [TutorId]             INT      NOT NULL,
    [TutorTimingConfigId] INT      NULL,
    [FromTimeId]          INT      NULL,
    [ToTimeId]            INT      NULL,
    [ScheduledDate]       DATETIME NULL,    
    [IsPaid]              BIT      NULL,
    [Status]              BIT      NULL,   
    [CreatedDttm] DATETIME       NULL,  
    [UpdatedBy]   INT       NULL,
    [UpdatedDttm] DATETIME       NULL,
    CONSTRAINT [PK_TutorSchedule] PRIMARY KEY CLUSTERED ([ScheduleId] ASC),
    CONSTRAINT [FK_StudentTutorSchedules_TimingMaster] FOREIGN KEY ([FromTimeId]) REFERENCES [dbo].[TimingMaster] ([TimingId]),
    CONSTRAINT [FK_StudentTutorSchedules_TimingMaster1] FOREIGN KEY ([ToTimeId]) REFERENCES [dbo].[TimingMaster] ([TimingId]),
    CONSTRAINT [FK_TutorSchedule_Student] FOREIGN KEY ([AppUserId]) REFERENCES [dbo].[AppUser] ([AppUserId]),
    CONSTRAINT [FK_TutorSchedule_TimeConfig] FOREIGN KEY ([TutorTimingConfigId]) REFERENCES [dbo].[TutorTimingConfig] ([TutorTimingId]),
    CONSTRAINT [FK_TutorSchedule_Tutor] FOREIGN KEY ([TutorId]) REFERENCES [dbo].[AppUser] ([AppUserId])
);




   







