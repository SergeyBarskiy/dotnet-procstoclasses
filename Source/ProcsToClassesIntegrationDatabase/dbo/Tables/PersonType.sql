CREATE TABLE [dbo].[PersonType] (
    [PersonTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [TypeName]     NVARCHAR (30) NOT NULL,
    CONSTRAINT [PK_PersonType] PRIMARY KEY CLUSTERED ([PersonTypeId] ASC)
);

