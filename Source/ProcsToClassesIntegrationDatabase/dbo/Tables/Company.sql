CREATE TABLE [dbo].[Company] (
    [CompanyId]   INT            IDENTITY (1, 1) NOT NULL,
    [City]        NVARCHAR (MAX) NULL,
    [CompanyName] NVARCHAR (30)  NOT NULL,
    [State]       NVARCHAR (MAX) NULL,
    [Street]      NVARCHAR (MAX) NULL,
    [Zip]         NVARCHAR (MAX) NULL,
    [RowVersionNumber] INT NOT NULL, 
    CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED ([CompanyId] ASC)
);

