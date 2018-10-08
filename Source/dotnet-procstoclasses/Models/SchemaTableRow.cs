using System;
using System.Data;
using System.Linq;

namespace ClassesFromStoredProcsGenerator.Models
{
    public class SchemaTableRow
    {
        public string ColumnName { get; set; }
        public int ColumnOrdinal { get; set; }
        public int ColumnSize { get; set; }
        public short NumericPrecision { get; set; }
        public short NumericScale { get; set; }
        public Type DataType { get; set; }
        public int ProviderType { get; set; }
        public bool IsLong { get; set; }
        public bool AllowDBNull { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsRowVersion { get; set; }
        public bool IsUnique { get; set; }
        public bool IsKey { get; set; }
        public bool IsAutoIncrement { get; set; }
        public string BaseCatalogName { get; set; }
        public string BaseSchemaName { get; set; }
        public string BaseTableName { get; set; }
        public string BaseColumnName { get; set; }
        public string FriendlyTypeName { get; set; }

        public override string ToString()
        {
            return $"{ColumnName} {FriendlyTypeName}";
        }

        public static SchemaTableRow FromDataRow(DataRow row)
        {
            var result = new SchemaTableRow();
            typeof(SchemaTableRow).GetProperties().ToList().ForEach(p =>
            {
                if (row.Table.Columns.IndexOf(p.Name) >= 0)
                {
                    var value = row[p.Name];
                    if (value == DBNull.Value)
                    {
                        value = null;
                    }
                    p.SetValue(result, value);
                }

            });
            return result;
        }


    }
}