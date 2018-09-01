namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Contracts;
    using Dtos;
    using Services.Contracts;

    public class LoginCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly IUserSessionService userSessionService;

        public LoginCommand(IUserService userService, IUserSessionService userSessionService)
        {
            this.userService = userService;
            this.userSessionService = userSessionService;
        }

        public string Execute(string[] args)
        {
            if (this.userSessionService.IsLoggedIn)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            string username = args[0];
            string password = args[1];

            UserDto user = this.userService.ByUsername<UserDto>(username);

            if (user == null || user.Password != password)
            {
                throw new ArgumentException("Invalid username or password!");
            }

            if (this.userSessionService.IsLoggedIn)
            {
                throw new ArgumentException("You should logout first!");
            }

            this.userSessionService.Login(username);

            return $"User {username} successfully logged in!";
        }
    }
}
