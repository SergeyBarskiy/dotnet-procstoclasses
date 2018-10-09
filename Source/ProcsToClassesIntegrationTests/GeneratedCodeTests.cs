using Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ProcsToClassesIntegrationTests
{
    [TestClass]
    public class GeneratedCodeTests
    {
        [TestMethod]
        public async Task Should_Execute_Data()
        {
            using (var connection = CreateConnection())
            {
                await connection.OpenAsync();
                var exec = new TestExecutor(new FirstClassMapper(), new SecondClassMapper());
                var data = await exec.Execute(new Abstractions.TestCriteria { CompanyId = 2 }, connection);
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
