
using System;
using System.Data.Common;
using Abstractions;

namespace Classes
{
	public class SecondClassMapper : ISecondClassMapper
	{
		public SecondClass Map(DbDataReader reader)
		{
			var result = new SecondClass();
			result.LastName = reader.GetString(0);
			result.FirstName = reader.GetString(1);
			result.PersonId = reader.GetInt32(2);
			result.CompanyId = reader.GetInt32(3);
			result.RowVersion = (byte[])reader[4];
 
			return result;
		}
	}
}
