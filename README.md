# dotnet-procstoclasses
DotNet tool to generate classes from stored procedures

To install type 

dotnet tool install --global dotnet-storedprocsgen

In order to run the generator, you must create a configuration file in a folder that you are running the generator from.  Here is an example of such file, by default named classes=config.json

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

wrappedData is the name of the class that is returned once procedure is executed.  It will have prorperties, one for each list correspnding to a single result set

executor is the name of the class that will execute the procedure

classes is the list of strings or class names, one for each result set, created in the order of result sets

locations will have to properties, for locations of interface and implementation fiels that are genereated

namespaces has two properties, again one for interfaces, one for implementations, containing namespace that these classes and interfaces will be put into

To run the generator just type 

procstoclasses -s SERVER -d DATABASE_NAME 

Two more paramters are optional:
-c FULL_PATH_AND_FILE_NAME_TO_CONFIG_JSON file as you see above

-p SINGLE_PROCEDURE_NAME_FROM_CONFIG_FILE
