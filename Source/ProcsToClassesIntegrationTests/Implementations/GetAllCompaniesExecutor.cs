
using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Abstractions;
namespace Classes
{
	public class GetAllCompaniesExecutor : IGetAllCompaniesExecutor
	{
		private readonly ICompanyInfoMapper _companyInfoMapper;


		public GetAllCompaniesExecutor(
			ICompanyInfoMapper companyInfoMapper
		)
		{
			_companyInfoMapper = companyInfoMapper;

		}

		public async Task<GetAllCompaniesData> Execute(DbConnection connection)
		{
			var result = new GetAllCompaniesData();
			result.CompanyInfoData = new List<CompanyInfo>();

			if (connection.State != System.Data.ConnectionState.Open)
			{
				await connection.OpenAsync();
			}
			using (var command = connection.CreateCommand())
			{
				command.CommandText = "usp_Company_Get_All";
				command.CommandType = System.Data.CommandType.StoredProcedure;

				using (DbDataReader reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						result.CompanyInfoData.Add(_companyInfoMapper.Map(reader));
					}

				}
			}
			return result;
		}
	}


}
