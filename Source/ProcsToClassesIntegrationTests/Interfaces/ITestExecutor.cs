
using System;
using System.Threading.Tasks;
using System.Data.Common;

namespace Abstractions
{
	public interface ITestExecutor
	{
		Task<WrappedData> Execute(TestCriteria criteria, DbConnection connection);
	}
}
