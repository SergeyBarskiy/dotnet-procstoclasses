using Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ProcsToClassesIntegrationTests
{
    [TestClass]
    public class GeneratedCodeTests
    {
        [TestMethod]
        public async Task Should_Execute_Parameterized_Procedure()
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                var exec = new TestExecutor(new FirstClassMapper(), new SecondClassMapper());
                var data = await exec.Execute(new Abstractions.TestCriteria { CompanyId = 2 }, connection);
                Assert.IsNotNull(data, "Should get the data");
            }
        }

        [TestMethod]
        public async Task Should_Execute_Parameterless_Procedure()
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                var exec = new GetAllCompaniesExecutor(new CompanyInfoMapper());
                var data = await exec.Execute(connection);
                Assert.IsNotNull(data, "Should get the data");
            }
        }

        public string CreateConnectionString()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = ".";
            builder.InitialCatalog = "ContactManager";
            builder.IntegratedSecurity = true;
            return builder.ConnectionString;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(CreateConnectionString());
        }
    }
}
