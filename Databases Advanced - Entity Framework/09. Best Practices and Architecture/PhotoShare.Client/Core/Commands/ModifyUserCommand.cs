namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;

    using Contracts;
    using Dtos;
    using Services.Contracts;

    public class ModifyUserCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly ITownService townService;
        private readonly IUserSessionService userSessionService;

        public ModifyUserCommand(
            IUserService userService, ITownService townService, IUserSessionService userSessionService)
        {
            this.userService = userService;
            this.townService = townService;
            this.userSessionService = userSessionService;
        }

        // ModifyUser <username> <property> <new value>
        // For example:
        // ModifyUser <username> Password <NewPassword>
        // ModifyUser <username> BornTown <newBornTownName>
        // ModifyUser <username> CurrentTown <newCurrentTownName>
        // !!! Cannot change username
        public string Execute(string[] args)
        {
            if (args.Length < 3)
            {
                throw new ArgumentException("Command ModifyUser not valid!");
            }

            string username = args[0];
            string property = args[1];
            string newValue = args[2];

            bool userExists = this.userService.Exists(username);

            if (!userExists)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            if (!this.userSessionService.IsLoggedIn || this.userSessionService.GetUsername != username)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            int userId = this.userService.ByUsername<UserDto>(username).Id;

            if (property == "Password")
            {
                this.SetPassword(userId, newValue);
            }
            else if (property == "BornTown")
            {
                this.SetBornTown(userId, newValue);
            }
            else if (property == "CurrentTown")
            {
                this.SetCurrentTown(userId, newValue);
            }
            else
            {
                throw new ArgumentException($"Property {property} not supported!");
            }

            return $"User {username} {property} is {newValue}.";
        }

        private void SetCurrentTown(int userId, string townName)
        {
            bool townExists = this.townService.Exists(townName);

            if (!townExists)
            {
                throw new ArgumentException(
                    $"Value {townName} not valid.{Environment.NewLine}Town {townName} not found!");
            }

            int townId = this.townService.ByName<TownDto>(townName).Id;

            this.userService.SetCurrentTown(userId, townId);
        }

        private void SetBornTown(int userId, string townName)
        {
            bool townExists = this.townService.Exists(townName);

            if (!townExists)
            {
                throw new ArgumentException(
                    $"Value {townName} not valid.{Environment.NewLine}Town {townName} not found!");
            }

            int townId = this.townService.ByName<TownDto>(townName).Id;

            this.userService.SetBornTown(userId, townId);
        }

        private void SetPassword(int userId, string newPassword)
        {
            bool hasLowerChar = newPassword.Any(c => char.IsLower(c));
            bool hasDigit = newPassword.Any(c => char.IsDigit(c));

            if (!hasLowerChar || !hasDigit)
            {
                throw new ArgumentException($"Value {newPassword} not valid.{Environment.NewLine}Invalid Password");
            }

            this.userService.ChangePassword(userId, newPassword);
        }
    }
}
