CREATE PROCEDURE [dbo].[usp_Company_Get_All]
AS
Select [City], [CompanyId], [CompanyName], [State], [Street], [Zip], [RowVersionNumber]
From Company
