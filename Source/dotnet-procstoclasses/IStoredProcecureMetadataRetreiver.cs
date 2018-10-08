using ClassesFromStoredProcsGenerator.Models;
using System.Data.SqlClient;

namespace ClassesFromStoredProcsGenerator
{
    public interface IStoredProcecureMetadataRetreiver
    {
        StoreProcedureMetadata GetMetadata(Procedure procedure, SqlConnection connection);
    }
}