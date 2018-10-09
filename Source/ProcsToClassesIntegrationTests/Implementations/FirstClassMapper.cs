
using System;
using System.Data.Common;
using Abstractions;

namespace Classes
{
	public class FirstClassMapper : IFirstClassMapper
	{
		public FirstClass Map(DbDataReader reader)
		{
			var result = new FirstClass();
			result.CompanyId = reader.GetInt32(0);
			result.City = reader.IsDBNull(1) ? null : reader.GetString(1);
			result.CompanyName = reader.GetString(2);
			result.State = reader.IsDBNull(3) ? null : reader.GetString(3);
			result.Street = reader.IsDBNull(4) ? null : reader.GetString(4);
			result.Zip = reader.IsDBNull(5) ? null : reader.GetString(5);
			result.RowVersionNumber = reader.GetInt32(6);
 
			return result;
		}
	}
}
