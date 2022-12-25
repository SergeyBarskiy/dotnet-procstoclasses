CREATE PROCEDURE [dbo].[usp_Company_GetById_With_Children]
	@CompanyId Int
AS
Select [CompanyId], [City], [CompanyName], [State], [Street], [Zip], [RowVersionNumber]
From Company
where Company.CompanyId = @CompanyId

Select [LastName], [FirstName], Person.PersonId, PersonCompany.CompanyId, [RowVersion] 
from Person
inner join PersonCompany on PersonCompany.PersonId = Person.PersonId
Where PersonCompany.CompanyId = @CompanyId