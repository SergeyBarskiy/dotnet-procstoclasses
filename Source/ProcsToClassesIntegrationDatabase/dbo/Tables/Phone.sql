CREATE TABLE [dbo].[Phone] (
    [PhoneId]     INT           IDENTITY (1, 1) NOT NULL,
    [PersonId]    INT           NOT NULL,
    [PhoneNumber] NVARCHAR (15) NOT NULL,
    CONSTRAINT [PK_Phone] PRIMARY KEY CLUSTERED ([PhoneId] ASC),
    CONSTRAINT [FK_Phone_Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Phone_PersonId]
    ON [dbo].[Phone]([PersonId] ASC);

