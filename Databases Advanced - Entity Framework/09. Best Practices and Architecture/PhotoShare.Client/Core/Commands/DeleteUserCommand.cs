namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Contracts;
    using Dtos;
    using Services.Contracts;

    public class DeleteUserCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly IUserSessionService userSessionService;

        public DeleteUserCommand(IUserService userService, IUserSessionService userSessionService)
        {
            this.userService = userService;
            this.userSessionService = userSessionService;
        }

        // DeleteUser <username>
        public string Execute(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentException("Command DeleteUser not valid!");
            }

            string username = args[0];

            bool userExists = this.userService.Exists(username);

            if (!userExists)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            if (!this.userSessionService.IsLoggedIn || this.userSessionService.GetUsername != username)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            UserDto user = this.userService.ByUsername<UserDto>(username);

            if (user.IsDeleted == true)
            {
                throw new InvalidOperationException($"User {username} is already deleted!");
            }

            this.userService.Delete(username);

            return $"User {username} was deleted successfully!";
        }
    }
}
