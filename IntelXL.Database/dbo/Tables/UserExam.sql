CREATE TABLE [dbo].[UserExam] (
    [UserExamId]     INT IDENTITY (1, 1) NOT NULL,
    [AppUserId]      INT NOT NULL,
    [QuestionId]     INT NOT NULL,
    [SubtopicId]     INT NOT NULL,
    [SubjectId]     INT NOT NULL, 
    [PracticeType]     VARCHAR(20) NOT NULL,
    [AnsweredStatus] INT NOT NULL,
    [CreatedDttm] DATETIME       NULL, 
    [UpdatedDttm] DATETIME       NULL,
    [YearOfQuestion]  VARCHAR(5) NULL,
    CONSTRAINT [PK_UserExam] PRIMARY KEY CLUSTERED ([UserExamId] ASC)
);

