namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Contracts;
    using Services.Contracts;

    public class AddTownCommand : ICommand
    {
        private readonly ITownService townService;
        private readonly IUserSessionService userSessionService;

        public AddTownCommand(ITownService townService, IUserSessionService userSessionService)
        {
            this.townService = townService;
            this.userSessionService = userSessionService;
        }

        // AddTown <townName> <countryName>
        public string Execute(string[] args)
        {
            if (args.Length < 2)
            {
                throw new ArgumentException("Command AddTown not valid!");
            }

            if (!this.userSessionService.IsLoggedIn)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            string townName = args[0];
            string country = args[1];

            bool townExists = this.townService.Exists(townName);

            if (townExists)
            {
                throw new ArgumentException($"Town {townName} was already added!");
            }

            this.townService.Add(townName, country);

            return $"Town {townName} was added successfully!";
        }
    }
}
