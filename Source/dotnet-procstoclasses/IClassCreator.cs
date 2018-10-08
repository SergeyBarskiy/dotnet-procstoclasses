using System.Collections.Generic;
using ClassesFromStoredProcsGenerator.Models;

namespace ClassesFromStoredProcsGenerator
{
    public interface IClassCreator
    {
        void CreateStoredProcedureAccessClasses(Procedure procedure, StoreProcedureMetadata metadata);
        string FirstCharToLower(string input);
        string FirstCharToUpper(string input);
        string GetClass(List<SchemaTableRow> parameters, string className, NamespaceData namespaceData);
        string GetCriteria(List<ParameterInfo> parameters, string criteriaClassName, NamespaceData namespaceData);
        string GetExecutorClass(Procedure procedure, StoreProcedureMetadata metadata);
        string GetExecutorInterface(Procedure procedure);
        string GetMapperClass(List<SchemaTableRow> parameters, string className, NamespaceData namespaceData);
        string GetMapperInterface(List<SchemaTableRow> parameters, string className, NamespaceData namespaceData);
        string GetWrapperData(Procedure procedure);
    }
}