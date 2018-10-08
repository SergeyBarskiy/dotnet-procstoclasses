using ClassesFromStoredProcsGenerator.Models;
using System.Collections.Generic;

namespace ClassesFromStoredProcsGenerator
{
    public interface ISqlTypeConverter
    {
        List<SqlTypeData> TypeList { get; }
    }
}
