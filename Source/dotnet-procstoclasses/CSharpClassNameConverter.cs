using System;
using System.Collections.Generic;

namespace ClassesFromStoredProcsGenerator
{
    public class CSharpClassNameConverter : ICSharpClassNameConverter
    {
        public string GetCSharpName(Type type, bool isNullable = false)
        {
            string result;
            if (!primitiveTypes.TryGetValue(type, out result))
            {
                result = type.ToString();
            }
            if (isNullable)
            {
                if (type != typeof(string))
                {
                    result = result + "?";
                }
            }
            return result;


        }

        public string GetReaderName(Type type)
        {
            string result;
            result = readerTypes[type];
            return result;


        }

        Dictionary<Type, string> primitiveTypes = new Dictionary<Type, string>
        {
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(char), "char" },
            { typeof(decimal), "decimal" },
            { typeof(double), "double" },
            { typeof(float), "float" },
            { typeof(int), "int" },
            { typeof(long), "long" },
            { typeof(sbyte), "sbyte" },
            { typeof(short), "short" },
            { typeof(string), "string" },
            { typeof(uint), "uint" },
            { typeof(ulong), "ulong" },
            { typeof(ushort), "ushort" },
            { typeof(byte[]), "byte[]" },
            { typeof(DateTime), "DateTime"}
        };

        Dictionary<Type, string> readerTypes = new Dictionary<Type, string>
        {
            { typeof(bool), "GetBoolean" },
            { typeof(byte), "GetByte" },
            { typeof(char), "GetChar" },
            { typeof(decimal), "GetDecimal" },
            { typeof(double), "GetDouble" },
            { typeof(float), "GetFloat" },
            { typeof(int), "GetInt32" },
            { typeof(long), "GetInt64" },
            { typeof(sbyte), "GetByte" },
            { typeof(short), "GetInt16" },
            { typeof(string), "GetString" },
            { typeof(uint), "GetInt32" },
            { typeof(ulong), "GetInt64" },
            { typeof(byte[]), "GetBytes" },
            { typeof(ushort), "GetInt16" },
            { typeof(DateTime), "GetDateTime" }
        };

    }
}