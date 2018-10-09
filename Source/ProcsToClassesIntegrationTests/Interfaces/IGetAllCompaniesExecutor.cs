
using System;
using System.Threading.Tasks;
using System.Data.Common;

namespace Abstractions
{
	public interface IGetAllCompaniesExecutor
	{
		Task<GetAllCompaniesData> Execute(DbConnection connection);
	}
}
