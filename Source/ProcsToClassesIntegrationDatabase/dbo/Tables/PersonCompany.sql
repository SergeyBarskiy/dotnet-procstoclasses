CREATE TABLE [dbo].[PersonCompany] (
    [CompanyId] INT NOT NULL,
    [PersonId]  INT NOT NULL,
    CONSTRAINT [PK_PersonCompany] PRIMARY KEY CLUSTERED ([CompanyId] ASC, [PersonId] ASC),
    CONSTRAINT [FK_PersonCompany_Company_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([CompanyId]) ON DELETE CASCADE,
    CONSTRAINT [FK_PersonCompany_Person_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person] ([PersonId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_PersonCompany_CompanyId]
    ON [dbo].[PersonCompany]([CompanyId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_PersonCompany_PersonId]
    ON [dbo].[PersonCompany]([PersonId] ASC);

