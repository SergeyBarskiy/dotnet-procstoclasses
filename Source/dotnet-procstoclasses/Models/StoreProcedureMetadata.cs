using System.Collections.Generic;

namespace ClassesFromStoredProcsGenerator.Models
{
    public class StoreProcedureMetadata
    {
        public List<ParameterInfo> Parameters { get; set; }
        public List<List<SchemaTableRow>> Results { get; set; }
        public StoreProcedureMetadata()
        {
            Parameters = new List<ParameterInfo>();
            Results = new List<List<SchemaTableRow>>();
        }
    }
}
