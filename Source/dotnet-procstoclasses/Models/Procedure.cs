using System.Collections.Generic;
namespace ClassesFromStoredProcsGenerator.Models
{
    public class Procedure
    {
        public string Name { get; set; }
        public Locations Locations { get; set; }
        public List<string> Classes { get; set; }
        public string Criteria { get; set; }
        public string Executor { get; set; }
        public string WrapperData { get; set; }
        public NamespaceData Namespaces { get; set; }
        
    }
}