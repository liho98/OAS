CREATE TABLE [dbo].[UserProfiles] (
    [UserId]      UNIQUEIDENTIFIER NOT NULL,
    [FirstName]   NVARCHAR (30)    NOT NULL,
    [LastName]    NVARCHAR (30)    NOT NULL,
    [Gender]      NCHAR (1)        NOT NULL,
    [ContactNo]   NVARCHAR (15)    NOT NULL,
    [DateOfBirth] DATE             NOT NULL,
    [Status]      NVARCHAR (50)    NULL,
    [Position]    NVARCHAR (50)    NULL,
    [ProgCode]    NCHAR (3)        NULL,
    [Image]       IMAGE            NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_UserProfiles_aspnet_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId])
);

CREATE TABLE [dbo].[Assessment] (
    [AssessmentId]    UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [AssessmentTitle] NVARCHAR (50)    NOT NULL,
    [AssessmentType]  NVARCHAR (50)    NULL,
    [AssessmentDesc]  NVARCHAR (100)   NULL,
    [UserId]          UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([AssessmentId] ASC),
    CONSTRAINT [FK_Assessment_UserProfiles] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfiles] ([UserId])
);

CREATE TABLE [dbo].[Answer] (
    [AssessmentId] UNIQUEIDENTIFIER NOT NULL,
    [UserId]       UNIQUEIDENTIFIER NOT NULL,
    [Score]        FLOAT (53)       NULL,
    PRIMARY KEY CLUSTERED ([AssessmentId] ASC, [UserId] ASC),
    CONSTRAINT [FK_Answer_Assessment] FOREIGN KEY ([AssessmentId]) REFERENCES [dbo].[Assessment] ([AssessmentId]),
    CONSTRAINT [FK_Answer_UserProfiles] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfiles] ([UserId])
);

CREATE TABLE [dbo].[Question] (
    [QuestionId]    UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [QuestionType]  NCHAR (10)       NULL,
    [QuestionText]  NVARCHAR (1000)  NULL,
    [QuestionLevel] NVARCHAR (15)    NULL,
    [CreatedDate]   DATE             NULL,
    [DefaultGrade]  NCHAR (1)        NULL,
    [Duration]      INT              NULL,
    [Image]         IMAGE            NULL,
    [AssessmentId]  UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([QuestionId] ASC),
    CONSTRAINT [FK_Question_Assessment] FOREIGN KEY ([AssessmentId]) REFERENCES [dbo].[Assessment] ([AssessmentId])
);

CREATE TABLE [dbo].[Option] (
    [OptionId]        UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [OptionText]      VARBINARY (100)  NULL,
    [IsCorrectAnswer] BIT              NULL,
    [Explanation]     NVARCHAR (250)   NULL,
    [Image]           IMAGE            NULL,
    [QuestionId]      UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([OptionId] ASC),
    CONSTRAINT [FK_Option_Question] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[Question] ([QuestionId])
);

