﻿namespace PhotoShare.Services
{
    using Contracts;
    using Models;

    public class UserSessionService : IUserSessionService
    {
        private readonly IUserService userService;

        public UserSessionService(IUserService userService)
        {
            this.userService = userService;
        }

        public User User { get; private set; }

        public string GetUsername => this.User.Username;

        public bool IsLoggedIn => this.User != null;

        public void Login(string username)
        {
            this.User = this.userService.ByUsername<User>(username);
        }

        public void Logout() => this.User = null;
    }
}
