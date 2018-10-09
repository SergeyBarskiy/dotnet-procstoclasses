using ClassesFromStoredProcsGenerator.Models;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ClassesFromStoredProcsGenerator
{
    [Command(
           Name = "dotnet procstoclasses",
           FullName = "dotnet-procstoclasses",
           Description = "Generates classes from stored procedures",
           ExtendedHelpText = "This tool generates classes from stored procedures.  Use -s to specify server name and -d for the database name. SSPI will be used. Use -c to specify custom confguration file.  If omitted, classes-config.json will be expected in current folder.")]
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

        [Option("-c|--config", CommandOptionType.SingleValue, Description = "Config file locatio and name.  Default is classes-config.json in current folder", ShowInHelpText = true)]
        public string Config { get; } = @".\classes-config.json";

        [Option("-p|--procedure", CommandOptionType.SingleValue, Description = "Singe procedure name to pick from config file to generate code for.", ShowInHelpText = true)]
        public string Procedure { get; } = "";

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
                    int total = 0;
                    Console.WriteLine("Connected...");
                    var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("classes-config.json"));
                    config.Procedures
                        .Where(prop => string.IsNullOrEmpty(Procedure) || prop.Name.ToUpper() == Procedure.ToUpper()).ToList()
                        .ForEach(procedure =>
                    {
                        Console.WriteLine($"Processing {procedure.Name}...");
                        var metadata = _storedProcecureMetadataRetreiver.GetMetadata(procedure, connection);
                        _classCreator.CreateStoredProcedureAccessClasses(procedure, metadata);
                        total++;
                    });
                    Console.WriteLine($"Completed. {total} procedures were processed");
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