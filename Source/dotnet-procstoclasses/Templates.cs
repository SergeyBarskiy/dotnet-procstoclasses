using System;
using System.Collections.Generic;
using System.Text;

namespace ClassesFromStoredProcsGenerator
{
    public static class Templates
    {
        public const string NamespaceTag = "_Namespace_";
        public const string ClassTag = "_Classname_";
        public const string PropertiesTag = "_Properties_";
        public const string ClassesNamepsace = "_ClassesInterface_";
        public const string ResultClass = "_Result_";
        public const string CriteriaClass = "_Criteria_";
        public const string InterfaceNamespaceTag = "_InterfacesNamespace_";
        public const string Mappers = "_Mappers_";
        public const string ConstructorParameters = "_Parameters_";
        public const string SetMembers = "_SetMembers_";
        public const string InitWrapper = "_InitWrapper_";
        public const string ProcedureName = "_ProcedureName_";
        public const string CommandParameters = "_CommandParamters_";
        public const string ReadResults = "_ReadResults_";

        public const string ClassTemplate = @"
using System;

namespace _Namespace_
{
    public class _Classname_
    {
_Properties_
    }
}
";
        public const string WrappperTemplate = @"
using System;
using System.Collections.Generic;

namespace _Namespace_
{
    public class _Classname_
    {
_Properties_
    }
}
";

        public const string ExecutorInterfaceTemplate = @"
using System;
using System.Threading.Tasks;
using System.Data.Common;

namespace _Namespace_
{
    public interface I_Classname_
    {
        Task<_Result_> Execute(_Criteria_ criteria, DbConnection connection);
    }
}
";

        public const string MapperTemplate = @"
using System;
using System.Data.Common;
using _InterfacesNamespace_;

namespace _Namespace_
{
    public class _Classname_Mapper : I_Classname_Mapper
    {
        public _Classname_ Map(DbDataReader reader)
        {
            var result = new _Classname_();
_Properties_ 
            return result;
        }
    }
}
";

        public const string IMapperTemplate = @"
using System;
using System.Data.Common;
namespace _Namespace_
{
    public interface I_Classname_Mapper
    {
        _Classname_ Map(DbDataReader reader);
    }
}
";

        public const string ExecutorClassTemplate = @"
using System.Data.Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using _InterfacesNamespace_;
namespace _Namespace_
{
    public class _Classname_ : I_Classname_
    {
_Mappers_

        public _Classname_(
_Parameters_
        )
        {
_SetMembers_
        }

        public async Task<_Result_> Execute(_Criteria_ criteria, DbConnection connection)
        {
            var result = new _Result_();
_InitWrapper_
            if (connection.State != System.Data.ConnectionState.Open)
            {
                await connection.OpenAsync();
            }
            using (var command = connection.CreateCommand())
            {
                command.CommandText = _ProcedureName_;
                command.CommandType = System.Data.CommandType.StoredProcedure;
_CommandParamters_
                using (DbDataReader reader = await command.ExecuteReaderAsync())
                {
_ReadResults_
                }
            }
            return result;
        }
    }


}
";

    }
}
