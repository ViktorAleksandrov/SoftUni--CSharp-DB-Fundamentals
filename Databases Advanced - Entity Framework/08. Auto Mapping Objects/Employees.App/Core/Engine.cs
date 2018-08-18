namespace Employees.App.Core
{
    using System;

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
            IDbInitializerService dbInitializer = this.serviceProvider.GetService<IDbInitializerService>();
            dbInitializer.InitializeDatabase();

            ICommandInterpreter commandInterpreter = this.serviceProvider.GetService<ICommandInterpreter>();

            while (true)
            {
                string[] input = Console.ReadLine()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                string result = commandInterpreter.Read(input);

                Console.WriteLine(result);
            }
        }
    }
}
