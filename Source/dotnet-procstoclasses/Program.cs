using McMaster.Extensions.CommandLineUtils;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ClassesFromStoredProcsGenerator
{
    class Program
    {
        // Return codes
        public const int EXCEPTION = 1;
        public const int OK = 0;

        public static int Main(string[] args)
        {
            try
            {

                var services = new ServiceCollection()
                    .AddSingleton<ICSharpClassNameConverter, CSharpClassNameConverter>()
                    .AddSingleton<ISqlTypeConverter, SqlTypeConverter>()
                    .AddSingleton<IClassCreator, ClassCreator>()
                    .AddSingleton<IStoredProcecureMetadataRetreiver, StoredProcecureMetadataRetreiver>()
                    .AddSingleton(PhysicalConsole.Singleton)
                    .BuildServiceProvider();

                var app = new CommandLineApplication<Generator>();
                app.Conventions
                    .UseDefaultConventions()
                    .UseConstructorInjection(services);

                return app.Execute(args);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Unexpected error: " + ex.ToString());
                Console.ResetColor();
                return EXCEPTION;
            }
        }
    }
}
