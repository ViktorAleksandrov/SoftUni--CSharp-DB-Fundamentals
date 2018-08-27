namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Contracts;
    using Services.Contracts;

    public class LogoutCommand : ICommand
    {
        private readonly IUserSessionService userSessionService;

        public LogoutCommand(IUserSessionService userSessionService)
        {
            this.userSessionService = userSessionService;
        }

        public string Execute(string[] args)
        {
            if (!this.userSessionService.IsLoggedIn)
            {
                throw new InvalidOperationException("You should log in first in order to logout.");
            }

            string username = this.userSessionService.GetUsername;

            this.userSessionService.Logout();

            return $"User {username} successfully logged out!";
        }
    }
}
