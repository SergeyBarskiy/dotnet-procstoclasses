using ClassesFromStoredProcsGenerator.Models;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace ClassesFromStoredProcsGenerator
{
    [Command(
           Name = "dotnet procstoclasses",
           FullName = "dotnet-procstoclasses",
           Description = "Generates stored procedures",
           ExtendedHelpText = "This tool generates classes from stored procedures.  Use -s to specify server name and -d for the database name.  SSPI will be used.")]
    [HelpOption]
    public partial class Generator
    {


        private readonly IClassCreator _classCreator;
        private readonly IStoredProcecureMetadataRetreiver _storedProcecureMetadataRetreiver;

        public Generator(IClassCreator classCreator, IStoredProcecureMetadataRetreiver storedProcecureMetadataRetreiver)
        {
            _classCreator = classCreator;
            _storedProcecureMetadataRetreiver = storedProcecureMetadataRetreiver;
        }

        [Required(ErrorMessage = "You must specify server name / -s or --server option")]
        [Option("-s|--server", CommandOptionType.SingleValue, Description = "Server name", ShowInHelpText = true)]
        public string Server { get; }

        [Required(ErrorMessage = "You must specify database name / -d or --database option")]
        [Option("-d|--database", CommandOptionType.SingleValue, Description = "Database name", ShowInHelpText = true)]
        public string Database { get; }

        [Option("-c|--config", CommandOptionType.SingleValue, Description = "Config file location", ShowInHelpText = true)]
        public string Config { get; } = @".\classes-config.json";

        public async Task<int> OnExecute(CommandLineApplication app, IConsole console)
        {
            var configFile = new FileInfo(Config);
            if (configFile.Exists)
            {
                Console.WriteLine($"Connecting to server {Server} to database {Database}...");
                Console.WriteLine($"... to create classes using configuration file {Config}");

                using (var connection = CreateConnection())
                {
                    await connection.OpenAsync();
                    Console.WriteLine("Connected...");
                    var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("classes-config.json"));
                    config.Procedures.ForEach(procedure =>
                    {
                        Console.WriteLine($"Processing {procedure.Name}...");
                        var metadata = _storedProcecureMetadataRetreiver.GetMetadata(procedure, connection);
                        _classCreator.CreateStoredProcedureAccessClasses(procedure, metadata);
                    });
                   
                }

                return Program.OK;
            }
            else
            {
                Console.WriteLine($"Cannot find configuration file {Config}");
                return Program.EXCEPTION;
            }
        }

        public string CreateConnectionString()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = Server;
            builder.InitialCatalog = Database;
            builder.IntegratedSecurity = true;
            return builder.ConnectionString;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(CreateConnectionString());
        }


    }
}