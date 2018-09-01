namespace PhotoShare.Client.Core
{
    using System;
    using System.Data.SqlClient;

    using Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using Services.Contracts;

    public class Engine : IEngine
    {
        private readonly IServiceProvider serviceProvider;

        public Engine(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Run()
        {
            IDatabaseInitializerService initializeService =
                this.serviceProvider.GetService<IDatabaseInitializerService>();

            initializeService.InitializeDatabase();

            ICommandInterpreter commandInterpreter = this.serviceProvider.GetService<ICommandInterpreter>();

            while (true)
            {
                try
                {
                    Console.WriteLine("Enter command:");

                    string[] input = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);

                    string result = commandInterpreter.Read(input);

                    Console.WriteLine(result);
                    Console.WriteLine();
                }
                catch (Exception exception) when (exception is SqlException || exception is ArgumentException ||
                                                  exception is InvalidOperationException)
                {
                    Console.WriteLine(exception.Message);
                    Console.WriteLine();
                }
            }
        }
    }
}