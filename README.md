# dotnet-procstoclasses
DotNet tool to generate C# classes from SQL Server stored procedures.  Those classes include the following: criteria class, model classes, one for each result set that a procedure returns and executor class that runs the stored procedure and returns the result.  The result class will contain a property for each result set.  All operations are asynchronous and target .NET Standard project.  You have to create a project ahead of time and needs to refernece System.Data.SqlClient NuGet.  Interfaces are generated for the executor class and for classes that map reader to models.

To install, type 

**dotnet tool install --global dotnet-procstoclasses**

In order to run the generator, you must create a configuration file in a folder that you are running the generator from.  Here is an example of such file, by default named classes-config.json

<pre>
{
  "procedures": [
    {
      "name": "usp_Company_GetById_With_Children",
      "criteria": "TestCriteria",
      "wrapperData": "WrappedData",
      "executor": "TestExecutor",
      "classes": [
        "FirstClass",
        "SecondClass"
      ],
      "locations": {
        "interfaces": "..\\..\\..\\..\\ProcsToClassesIntegrationTests\\Interfaces",
        "implementations": "..\\..\\..\\..\\ProcsToClassesIntegrationTests\\Implementations"
      },
      "namespaces": {
        "classes": "Classes",
        "interfaces": "Abstractions"
      }
    }
  ]
}
</pre>
It contains a list of procedure objects.  All fields are required.

name is just procedure's name

criteria is the name of the class for criteria object. Leave blank for procedures without parameters

wrappedData is the name of the class that is returned once procedure is executed.  It will have properties, one for each list correspnding to a single result set

executor is the name of the class that will execute the procedure

classes is the list of strings aka class names, one for each result set, created in the order of result sets

locations will have to properties, for locations of interfaces and implementations classes or files that are generated

namespaces has two properties, again one for interfaces, one for implementations, containing namespace that these classes and interfaces will be put into

To run the generator just type 

**procstoclasses -s SERVER -d DATABASE_NAME**
SERVER is the SQL Server instance, such as(local) or ..  DATABASE_NAME is the name of the database.  Windows security will be used for conencting, hence no user or password parameters.

Two more paramters are optional:
-c FULL_PATH_AND_FILE_NAME_TO_CONFIG_JSON file as you see above

-p SINGLE_PROCEDURE_NAME_FROM_CONFIG_FILE.  Use this to generate classes for just one procedure.
