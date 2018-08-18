namespace Employees.App.Core
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Contracts;

    public class CommandInterpreter : ICommandInterpreter
    {
        private readonly IServiceProvider serviceProvider;

        public CommandInterpreter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public string Read(string[] input)
        {
            string commandName = input[0] + "Command";

            Type type = Assembly.GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name == commandName);

            if (type == null)
            {
                throw new ArgumentException("Invalid command");
            }

            ConstructorInfo constructor = type.GetConstructors().First();

            Type[] ctorParams = constructor.GetParameters()
                .Select(p => p.ParameterType)
                .ToArray();

            object[] services = ctorParams.Select(this.serviceProvider.GetService).ToArray();

            var command = (ICommand)constructor.Invoke(services);

            string[] args = input.Skip(1).ToArray();

            string result = command.Execute(args);

            return result;
        }
    }
}
