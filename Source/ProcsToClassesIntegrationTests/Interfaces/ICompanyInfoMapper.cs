
using System;
using System.Data.Common;
namespace Abstractions
{
	public interface ICompanyInfoMapper
	{
		CompanyInfo Map(DbDataReader reader);
	}
}
