
using System;
using System.Data.Common;
using Abstractions;

namespace Classes
{
	public class CompanyInfoMapper : ICompanyInfoMapper
	{
		public CompanyInfo Map(DbDataReader reader)
		{
			var result = new CompanyInfo();
			result.City = reader.IsDBNull(0) ? null : reader.GetString(0);
			result.CompanyId = reader.GetInt32(1);
			result.CompanyName = reader.GetString(2);
 
			return result;
		}
	}
}
