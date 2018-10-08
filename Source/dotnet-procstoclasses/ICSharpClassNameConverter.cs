using System;

namespace ClassesFromStoredProcsGenerator
{
    public interface ICSharpClassNameConverter
    {
        string GetCSharpName(Type type, bool isNullable = false);
        string GetReaderName(Type type);
    }
}