using ClassesFromStoredProcsGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClassesFromStoredProcsGenerator
{
    public class ClassCreator : IClassCreator
    {
        private readonly ISqlTypeConverter _sqlTypeConverter;
        private readonly ICSharpClassNameConverter _cSharpClassNameConverter;

        public ClassCreator(ISqlTypeConverter sqlTypeConverter, ICSharpClassNameConverter cSharpClassNameConverter)
        {
            _sqlTypeConverter = sqlTypeConverter;
            _cSharpClassNameConverter = cSharpClassNameConverter;
        }
        public void CreateStoredProcedureAccessClasses(Procedure procedure, StoreProcedureMetadata metadata)
        {
            FileInfo file;
            if (!string.IsNullOrEmpty(procedure.Criteria))
            {
                var criteria = GetCriteria(metadata.Parameters, procedure.Criteria, procedure.Namespaces);
                var criteriaFile = Path.Combine(procedure.Locations.Interfaces, procedure.Criteria + ".cs");
                file = new FileInfo(criteriaFile);
                file.Directory.Create();
                File.WriteAllText(criteriaFile, criteria);
            }

            var wrapperData = GetWrapperData(procedure);
            var wrapperDataFile = Path.Combine(procedure.Locations.Interfaces, procedure.WrapperData + ".cs");
            file = new FileInfo(wrapperDataFile);
            file.Directory.Create();
            File.WriteAllText(wrapperDataFile, wrapperData);

            var executorInterfaceData = GetExecutorInterface(procedure);
            var executorInterfaceFile = Path.Combine(procedure.Locations.Interfaces, "I" + procedure.Executor + ".cs");
            file = new FileInfo(executorInterfaceFile);
            file.Directory.Create();
            File.WriteAllText(executorInterfaceFile, executorInterfaceData);

            for (int i = 0; i < metadata.Results.Count; i++)
            {
                var oneClass = GetClass(metadata.Results[i], procedure.Classes[i], procedure.Namespaces);
                var oneClassFile = Path.Combine(procedure.Locations.Interfaces, procedure.Classes[i] + ".cs");
                file = new FileInfo(oneClassFile);
                file.Directory.Create();
                File.WriteAllText(oneClassFile, oneClass);

                oneClass = GetMapperInterface(metadata.Results[i], procedure.Classes[i], procedure.Namespaces);
                oneClassFile = Path.Combine(procedure.Locations.Interfaces, $"I{procedure.Classes[i]}Mapper.cs");
                file = new FileInfo(oneClassFile);
                file.Directory.Create();
                File.WriteAllText(oneClassFile, oneClass);

                oneClass = GetMapperClass(metadata.Results[i], procedure.Classes[i], procedure.Namespaces);
                oneClassFile = Path.Combine(procedure.Locations.Implementations, $"{procedure.Classes[i]}Mapper.cs");
                file = new FileInfo(oneClassFile);
                file.Directory.Create();
                File.WriteAllText(oneClassFile, oneClass);
            }

            var executorClassData = GetExecutorClass(procedure, metadata);
            var executorClassFile = Path.Combine(procedure.Locations.Implementations, procedure.Executor + ".cs");
            file = new FileInfo(executorClassFile);
            file.Directory.Create();
            File.WriteAllText(executorClassFile, executorClassData);
        }

        public string GetWrapperData(Procedure procedure)
        {
            var result = new StringBuilder();
            result.Append(Templates.WrappperTemplate.Replace(Templates.ClassTag, procedure.WrapperData)
                .Replace(Templates.NamespaceTag, procedure.Namespaces.Interfaces))
                .Replace("    ", "\t");
            var parametersSection = new StringBuilder();
            procedure.Classes.ForEach(p =>
            {
                parametersSection.AppendLine($"\t\tpublic List<{p}> {p}Data {{ get; set; }}");
            });

            return result.ToString().Replace(Templates.PropertiesTag, parametersSection.ToString());
        }

        public string GetExecutorInterface(Procedure procedure)
        {
            var result = new StringBuilder();
            result.Append(Templates.ExecutorInterfaceTemplate
                .Replace(Templates.ClassTag, procedure.Executor)
                .Replace(Templates.ResultClass, procedure.WrapperData)
                .Replace(Templates.CriteriaClass, procedure.Criteria)
                .Replace(Templates.NamespaceTag, procedure.Namespaces.Interfaces))
                .Replace("    ", "\t");
            var output = result.ToString();
            if (string.IsNullOrEmpty(procedure.Criteria))
            {
                output = output.Replace(@" criteria, ", "");
            }
            return output;
        }

        public string GetExecutorClass(Procedure procedure, StoreProcedureMetadata metadata)
        {
            var result = new StringBuilder();
            result.Append(Templates.ExecutorClassTemplate
                .Replace(Templates.ClassTag, procedure.Executor)
                .Replace(Templates.ResultClass, procedure.WrapperData)
                .Replace(Templates.CriteriaClass, procedure.Criteria)
                .Replace(Templates.InterfaceNamespaceTag, procedure.Namespaces.Interfaces)
                .Replace(Templates.NamespaceTag, procedure.Namespaces.Classes))
                .Replace("    ", "\t");
            var privateMembers = new StringBuilder();
            procedure.Classes.ForEach(c =>
            {
                privateMembers.AppendLine($"\t\tprivate readonly I{c}Mapper _{FirstCharToLower(c)}Mapper;");
            });

            var contructorParameters = new StringBuilder();
            procedure.Classes.ForEach(c =>
            {
                string comma = "," + Environment.NewLine;
                if (procedure.Classes.IndexOf(c) == procedure.Classes.Count - 1)
                {
                    comma = "";
                }
                contructorParameters.Append($"\t\t\tI{c}Mapper {FirstCharToLower(c)}Mapper{comma}");
            });

            var setMemebers = new StringBuilder();
            procedure.Classes.ForEach(c =>
            {
                setMemebers.AppendLine($"\t\t\t_{FirstCharToLower(c)}Mapper = {FirstCharToLower(c)}Mapper;");
            });

            var initWrapper = new StringBuilder();
            procedure.Classes.ForEach(c =>
            {
                initWrapper.AppendLine($"\t\t\tresult.{c}Data = new List<{c}>();");
            });

            var commandParameters = new StringBuilder();
            metadata.Parameters.ForEach(p =>
            {
                bool isFirst = metadata.Parameters.IndexOf(p) == 0;
                if (isFirst)
                {
                    commandParameters.AppendLine($"\t\t\t\tvar param = command.CreateParameter();");
                }
                else
                {
                    commandParameters.AppendLine($"\t\t\t\tparam = command.CreateParameter();");
                }
                commandParameters.AppendLine($"\t\t\t\tparam.ParameterName = \"{p.ParameterName}\";");
                var type = _sqlTypeConverter.TypeList.First(t => t.SqlServerTypeName == p.TypeName);
                commandParameters.AppendLine($"\t\t\t\tparam.DbType = System.Data.DbType.{type.DbType.ToString()};");
                if (type.IsMaxLengthNeeded)
                {
                    commandParameters.AppendLine($"\t\t\t\tparam.Size = {p.ParameterMaxBytes};");
                }
                if (type.IsPrecisionNeeded)
                {
                    commandParameters.AppendLine($"\t\t\t\tparam.Precision = {p.ParameterPrecision};");
                }
                if (type.IsScaleNeeded)
                {
                    commandParameters.AppendLine($"\t\t\t\tparam.Precision = {p.ParameterScale};");
                }
                var value = p.ParameterName.Replace("@", "");
                commandParameters.AppendLine($"\t\t\t\tparam.Value = criteria.{value};");

                if (Nullable.GetUnderlyingType(p.DotNetType) != null || p.DotNetType == typeof(string) || p.IsNullable)
                {
                    commandParameters.AppendLine($"\t\t\t\tif(criteria.{value} == null)");
                    commandParameters.AppendLine("\t\t\t\t{");
                    commandParameters.AppendLine("\t\t\t\t\tparam.Value = DBNull.Value;");
                    commandParameters.AppendLine("\t\t\t\t}");
                }
                commandParameters.AppendLine("\t\t\t\tcommand.Parameters.Add(param);");
            });

            var readResults = new StringBuilder();
            procedure.Classes.ForEach(c =>
            {
                bool isFirst = procedure.Classes.IndexOf(c) == 0;
                if (!isFirst)
                {
                    readResults.AppendLine($"\t\t\t\t\tawait reader.NextResultAsync();");
                }
                readResults.AppendLine("\t\t\t\t\twhile (await reader.ReadAsync())");
                readResults.AppendLine("\t\t\t\t\t{");
                readResults.AppendLine($"\t\t\t\t\t\tresult.{c}Data.Add(_{FirstCharToLower(c)}Mapper.Map(reader));");
                readResults.AppendLine("\t\t\t\t\t}");
            });

            var output = result.ToString()
                .Replace(Templates.Mappers, privateMembers.ToString())
                .Replace(Templates.ConstructorParameters, contructorParameters.ToString())
                .Replace(Templates.SetMembers, setMemebers.ToString())
                .Replace(Templates.InitWrapper, initWrapper.ToString())
                .Replace(Templates.CommandParameters, commandParameters.ToString())
                .Replace(Templates.ReadResults, readResults.ToString())
                .Replace(Templates.ProcedureName, @"""" + procedure.Name + @"""");

            if (string.IsNullOrEmpty(procedure.Criteria))
            {
                output = output.Replace(@" criteria, ", "");
            }
            return output;
        }

        public string GetCriteria(List<ParameterInfo> parameters, string criteriaClassName, NamespaceData namespaceData)
        {
            var result = new StringBuilder();
            result.Append(Templates.ClassTemplate.Replace(Templates.ClassTag, criteriaClassName)
                .Replace(Templates.NamespaceTag, namespaceData.Interfaces))
                .Replace("    ", "\t");
            var parametersSection = new StringBuilder();
            parameters.ForEach(p =>
            {
                parametersSection.AppendLine($"\t\tpublic {_cSharpClassNameConverter.GetCSharpName(p.DotNetType, p.IsNullable)} {FirstCharToUpper(p.ParameterName)} {{ get; set; }}");
            });

            return result.ToString().Replace(Templates.PropertiesTag, parametersSection.ToString());
        }

        public string GetClass(List<SchemaTableRow> parameters, string className, NamespaceData namespaceData)
        {
            var result = new StringBuilder();
            result.Append(Templates.ClassTemplate.Replace(Templates.ClassTag, className)
                .Replace(Templates.NamespaceTag, namespaceData.Interfaces))
                .Replace("    ", "\t");
            var parametersSection = new StringBuilder();
            parameters.ForEach(p =>
            {
                parametersSection.AppendLine($"\t\tpublic {_cSharpClassNameConverter.GetCSharpName(p.DataType, p.AllowDBNull)} {FirstCharToUpper(p.ColumnName)} {{ get; set; }}");
            });

            return result.ToString().Replace(Templates.PropertiesTag, parametersSection.ToString());
        }

        public string GetMapperInterface(List<SchemaTableRow> parameters, string className, NamespaceData namespaceData)
        {
            var result = new StringBuilder();
            result.Append(Templates.IMapperTemplate.Replace(Templates.ClassTag, className)
                .Replace(Templates.NamespaceTag, namespaceData.Interfaces))
                .Replace("    ", "\t");
            var parametersSection = new StringBuilder();

            return result.ToString();
        }

        public string GetMapperClass(List<SchemaTableRow> parameters, string className, NamespaceData namespaceData)
        {
            var result = new StringBuilder();
            result.Append(Templates.MapperTemplate.Replace(Templates.ClassTag, className)
                .Replace(Templates.NamespaceTag, namespaceData.Classes)
                .Replace(Templates.InterfaceNamespaceTag, namespaceData.Interfaces))
                .Replace("    ", "\t");
            var parametersSection = new StringBuilder();
            parameters.ForEach(p =>
            {
                var reader = _cSharpClassNameConverter.GetReaderName(p.DataType);
                if (p.DataType == typeof(byte[]))
                {
                    if (p.AllowDBNull)
                    {
                        parametersSection.AppendLine($"\t\t\tresult.{p.ColumnName} = reader.IsDBNull({p.ColumnOrdinal}) ? null : (byte[])reader[{p.ColumnOrdinal}];");
                    }
                    else
                    {
                        parametersSection.AppendLine($"\t\t\tresult.{p.ColumnName} = (byte[])reader[{p.ColumnOrdinal}];");
                    }
                }
                else if (p.DataType == typeof(int) && p.AllowDBNull)
                {
                    parametersSection.AppendLine($"\t\t\tresult.{p.ColumnName} = reader.IsDBNull({p.ColumnOrdinal}) ? (int?)null : reader.{reader}({p.ColumnOrdinal});");
                }
                else if (p.DataType == typeof(short) && p.AllowDBNull)
                {
                    parametersSection.AppendLine($"\t\t\tresult.{p.ColumnName} = reader.IsDBNull({p.ColumnOrdinal}) ? (short?)null : reader.{reader}({p.ColumnOrdinal});");
                }
                else if (p.DataType == typeof(long) && p.AllowDBNull)
                {
                    parametersSection.AppendLine($"\t\t\tresult.{p.ColumnName} = reader.IsDBNull({p.ColumnOrdinal}) ? (long?)null : reader.{reader}({p.ColumnOrdinal});");
                }
                else if (p.DataType == typeof(DateTime) && p.AllowDBNull)
                {
                    parametersSection.AppendLine($"\t\t\tresult.{p.ColumnName} = reader.IsDBNull({p.ColumnOrdinal}) ? (DateTime?)null : reader.{reader}({p.ColumnOrdinal});");
                }
                else if (p.DataType == typeof(bool) && p.AllowDBNull)
                {
                    parametersSection.AppendLine($"\t\t\tresult.{p.ColumnName} = reader.IsDBNull({p.ColumnOrdinal}) ? (bool?)null : reader.{reader}({p.ColumnOrdinal});");
                }

                else if (p.DataType == typeof(decimal) && p.AllowDBNull)
                {
                    parametersSection.AppendLine($"\t\t\tresult.{p.ColumnName} = reader.IsDBNull({p.ColumnOrdinal}) ? (decimal?)null : reader.{reader}({p.ColumnOrdinal});");
                }
                else
                {
                    if (p.AllowDBNull)
                    {
                        parametersSection.AppendLine($"\t\t\tresult.{p.ColumnName} = reader.IsDBNull({p.ColumnOrdinal}) ? null : reader.{reader}({p.ColumnOrdinal});");
                    }
                    else
                    {
                        parametersSection.AppendLine($"\t\t\tresult.{p.ColumnName} = reader.{reader}({p.ColumnOrdinal});");
                    }
                }

            });
            result = result.Replace(Templates.PropertiesTag, parametersSection.ToString());
            return result.ToString();
        }

        public string FirstCharToUpper(string input)
        {
            var result = input;
            if (!string.IsNullOrEmpty(input))
            {
                result = input.First().ToString().ToUpper() + input.Substring(1);
                result = result.Replace("@", "");
            }
            return result;
        }

        public string FirstCharToLower(string input)
        {
            var result = input;
            if (!string.IsNullOrEmpty(input))
            {
                result = input.First().ToString().ToLower() + input.Substring(1);
                result = result.Replace("@", "");
            }
            return result;
        }
    }
}
