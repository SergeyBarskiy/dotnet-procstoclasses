
using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Abstractions;
namespace Classes
{
	public class TestExecutor : ITestExecutor
	{
		private readonly IFirstClassMapper _firstClassMapper;
		private readonly ISecondClassMapper _secondClassMapper;


		public TestExecutor(
			IFirstClassMapper firstClassMapper,
			ISecondClassMapper secondClassMapper
		)
		{
			_firstClassMapper = firstClassMapper;
			_secondClassMapper = secondClassMapper;

		}

		public async Task<WrappedData> Execute(TestCriteria criteria, DbConnection connection)
		{
			var result = new WrappedData();
			result.FirstClassData = new List<FirstClass>();
			result.SecondClassData = new List<SecondClass>();

			if (connection.State != System.Data.ConnectionState.Open)
			{
				await connection.OpenAsync();
			}
			using (var command = connection.CreateCommand())
			{
				command.CommandText = "usp_Company_GetById_With_Children";
				command.CommandType = System.Data.CommandType.StoredProcedure;
				var param = command.CreateParameter();
				param.ParameterName = "@CompanyId";
				param.DbType = System.Data.DbType.Int32;
				param.Value = criteria.CompanyId;
				if(criteria.CompanyId == null)
				{
					param.Value = DBNull.Value;
				}
				command.Parameters.Add(param);

				using (DbDataReader reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						result.FirstClassData.Add(_firstClassMapper.Map(reader));
					}
					await reader.NextResultAsync();
					while (await reader.ReadAsync())
					{
						result.SecondClassData.Add(_secondClassMapper.Map(reader));
					}

				}
			}
			return result;
		}
	}


}
