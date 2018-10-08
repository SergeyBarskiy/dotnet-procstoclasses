using System;
using System.Data;

namespace ClassesFromStoredProcsGenerator.Models
{
    public class SqlTypeData
    {
        public string SqlServerTypeName { get; private set; }
        public DbType DbType { get; private set; }
        public Type DotNetType { get; private set; }
        public bool IsMaxLengthNeeded { get; private set; }
        public bool IsScaleNeeded { get; private set; }
        public bool IsPrecisionNeeded { get; private set; }


        public SqlTypeData(string sqlServerTypeName, DbType dbType, Type dotNetType, bool isMaxLengthNeeded = false, bool isScaleNeeded = false, bool isPrecisionNeeded = false)
        {
            SqlServerTypeName = sqlServerTypeName;
            DbType = dbType;
            DotNetType = dotNetType;
            IsMaxLengthNeeded = IsMaxLengthNeeded;
            IsScaleNeeded = IsScaleNeeded;
            isPrecisionNeeded = IsPrecisionNeeded;
        }
    }
}