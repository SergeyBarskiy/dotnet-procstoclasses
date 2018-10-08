

using System;

namespace ClassesFromStoredProcsGenerator.Models
{
    public class ParameterInfo
    {
        public string ParameterName { get; set; }
        public string TypeName { get; set; }
        public short ParameterMaxBytes { get; set; }
        public byte ParameterPrecision { get; set; }
        public byte ParameterScale { get; set; }
        public bool IsNullable { get; set; }
        public Type DotNetType { get; set; }
    }
}
