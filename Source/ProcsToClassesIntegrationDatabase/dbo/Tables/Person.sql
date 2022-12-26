CREATE TABLE [dbo].[Person] (
    [PersonId]     INT           IDENTITY (1, 1) NOT NULL,
    [BirthDate]    DATETIME2 (7) NULL,
    [FirstName]    NVARCHAR (30) NOT NULL,
    [IsActive]     BIT           NOT NULL,
    [LastName]     NVARCHAR (40) NOT NULL,
    [PersonTypeId] INT           NOT NULL,
    [RowVersion] ROWVERSION NOT NULL, 
    CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([PersonId] ASC),
    CONSTRAINT [FK_Person_PersonType_PersonTypeId] FOREIGN KEY ([PersonTypeId]) REFERENCES [dbo].[PersonType] ([PersonTypeId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Person_PersonTypeId]
    ON [dbo].[Person]([PersonTypeId] ASC);

