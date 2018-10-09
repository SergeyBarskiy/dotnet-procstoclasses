using ClassesFromStoredProcsGenerator.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ClassesFromStoredProcsGenerator
{
    public class StoredProcecureMetadataRetreiver : IStoredProcecureMetadataRetreiver
    {
        private readonly ISqlTypeConverter _sqlTypeConverter;
        private readonly ICSharpClassNameConverter _cSharpClassNameConverter;

        public StoredProcecureMetadataRetreiver(ISqlTypeConverter sqlTypeConverter, ICSharpClassNameConverter cSharpClassNameConverter)
        {
            _sqlTypeConverter = sqlTypeConverter;
            _cSharpClassNameConverter = cSharpClassNameConverter;
        }
        public StoreProcedureMetadata GetMetadata(Procedure procedure, SqlConnection connection)
        {
            var result = new StoreProcedureMetadata();

            PopulateParamters(procedure, result, connection);
            PopulateResults(procedure, result, connection);

            return result;
        }

        private void PopulateResults(Procedure procedure, StoreProcedureMetadata result, SqlConnection conn)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = procedure.Name;
                cmd.CommandType = CommandType.StoredProcedure;
                result.Parameters.ForEach(p =>
                {
                    var dotNetType = _sqlTypeConverter.TypeList.First(_ => _.SqlServerTypeName == p.TypeName).DotNetType;
                    var value = (object)DBNull.Value;
                    if (dotNetType != null)
                    {
                        if (dotNetType == typeof(string))
                        {
                            value = "";
                        }
                        else
                        {
                            value = Activator.CreateInstance(dotNetType);
                        }
                    }

                    cmd.Parameters.AddWithValue(p.ParameterName, value);
                });
                var listOfResults = new List<DataTable>();
                using (var reader = cmd.ExecuteReader())
                {
                    listOfResults.Add(reader.GetSchemaTable());
                    while (reader.NextResult())
                    {
                        listOfResults.Add(reader.GetSchemaTable());
                    }
                }

                listOfResults.ForEach(r =>
                {
                    var singleResultData = new List<SchemaTableRow>();
                    foreach (DataRow row in r.Rows)
                    {
                        var newRow = SchemaTableRow.FromDataRow(row);
                        newRow.FriendlyTypeName = _cSharpClassNameConverter.GetCSharpName(newRow.DataType);
                        singleResultData.Add(newRow);
                    }
                    result.Results.Add(singleResultData);
                });
            }
        }

        private void PopulateParamters(Procedure procedure, StoreProcedureMetadata result, SqlConnection conn)
        {
            var procedureName = procedure.Name;

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
select  
name as ParameterName,  
type_name(user_type_id) as TypeName,  
max_length as ParameterMaxBytes, 
precision as ParameterPrecision,
scale as ParameterScale,
is_nullable as IsNullable
from sys.parameters where object_id = object_id(@procedureName)
order by parameter_id
";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@procedureName", procedureName);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var param = new ParameterInfo();
                        typeof(ParameterInfo).GetProperties().Where(p => p.Name != "DotNetType").ToList().ForEach(p =>
                        {
                            p.SetValue(param, reader[p.Name]);
                        });
                        var dotNetType = _sqlTypeConverter.TypeList.First(_ => _.SqlServerTypeName == param.TypeName).DotNetType;
                        param.DotNetType = dotNetType;
                        result.Parameters.Add(param);
                    }
                }
            }
        }
    }
}
