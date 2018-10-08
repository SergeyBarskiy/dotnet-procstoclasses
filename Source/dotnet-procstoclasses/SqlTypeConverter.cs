using ClassesFromStoredProcsGenerator.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace ClassesFromStoredProcsGenerator
{
    public class SqlTypeConverter : ISqlTypeConverter
    {
        private List<SqlTypeData> _typeList = new List<SqlTypeData>
        {
            new SqlTypeData("bigint", DbType.Int64,typeof(long)),
            new SqlTypeData("binary", DbType.Binary,typeof(byte[]), true),
            new SqlTypeData("bit", DbType.Boolean,typeof(bool)),
            new SqlTypeData("char", DbType.AnsiStringFixedLength,typeof(char), true),
            new SqlTypeData("cursor", DbType.Object,null),
            new SqlTypeData("date", DbType.Date,typeof(DateTime)),
            new SqlTypeData("datetime", DbType.DateTime,typeof(DateTime)),
            new SqlTypeData("datetime2", DbType.DateTime2,typeof(DateTime), false, true, false),
            new SqlTypeData("datetimeoffset", DbType.DateTimeOffset,typeof(DateTimeOffset), false, true, false),
            new SqlTypeData("decimal", DbType.Decimal,typeof(Decimal), false, true, true),
            new SqlTypeData("float", DbType.Double,typeof(double)),
            new SqlTypeData("geography",DbType.Object,typeof(byte[])),
            new SqlTypeData("geometry", DbType.Object,typeof(byte[])),
            new SqlTypeData("hierarchyid", DbType.Object,typeof(byte[])),
            new SqlTypeData("image", DbType.Binary,typeof(byte[])),
            new SqlTypeData("int", DbType.Int32,typeof(int)),
            new SqlTypeData("money", DbType.Currency,typeof(decimal)),
            new SqlTypeData("nchar", DbType.StringFixedLength,typeof(string), true),
            new SqlTypeData("ntext", DbType.String,typeof(string)),
            new SqlTypeData("numeric",DbType.Decimal,typeof(decimal), false, true, true),
            new SqlTypeData("nvarchar", DbType.String,typeof(string), true),
            new SqlTypeData("real", DbType.Single,typeof(float)),
            new SqlTypeData("rowversion", DbType.Binary,typeof(byte[])),
            new SqlTypeData("smallint", DbType.Int16,typeof(short)),
            new SqlTypeData("smallmoney", DbType.Currency,typeof(decimal)),
            new SqlTypeData("sql_variant", DbType.Object,typeof(object)),
            new SqlTypeData("table", DbType.Object,typeof(object)),
            new SqlTypeData("text", DbType.AnsiString,typeof(string)),
            new SqlTypeData("time", DbType.Time,typeof(TimeSpan), false, true, false),
            new SqlTypeData("timestamp", DbType.Binary,typeof(byte[])),
            new SqlTypeData("tinyint",DbType.Byte,typeof(byte)),
            new SqlTypeData("uniqueidentifier", DbType.Guid,typeof(Guid)),
            new SqlTypeData("varbinary", DbType.Binary,typeof(byte[]), true),
            new SqlTypeData("varchar", DbType.String,typeof(string), true),
            new SqlTypeData("xml", DbType.Xml,typeof(string))
        };

        List<SqlTypeData> ISqlTypeConverter.TypeList => _typeList;
    }
}
