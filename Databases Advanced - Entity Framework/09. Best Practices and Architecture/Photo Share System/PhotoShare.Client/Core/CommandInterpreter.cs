namespace PhotoShare.Client.Core
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
            string inputCommand = input[0] + "Command";

            if (inputCommand == "MakeFriendsCommand")
            {
                inputCommand = "AddFriendCommand";
            }
            else if (inputCommand == "ListFriendsCommand")
            {
                inputCommand = "PrintFriendsListCommand";
            }

            string[] args = input.Skip(1).ToArray();

            Type type = Assembly.GetCallingAssembly()
                               .GetTypes()
                               .FirstOrDefault(x => x.Name == inputCommand);

            if (type == null)
            {
                throw new InvalidOperationException($"Command {input[0]} not valid!");
            }

            ConstructorInfo constructor = type.GetConstructors()
                                  .First();

            Type[] constructorParameters = constructor.GetParameters()
                                                   .Select(x => x.ParameterType)
                                                   .ToArray();

            object[] service = constructorParameters.Select(this.serviceProvider.GetService)
                                               .ToArray();

            var command = (ICommand)constructor.Invoke(service);

            string result = command.Execute(args);

            return result;
        }
    }
}
