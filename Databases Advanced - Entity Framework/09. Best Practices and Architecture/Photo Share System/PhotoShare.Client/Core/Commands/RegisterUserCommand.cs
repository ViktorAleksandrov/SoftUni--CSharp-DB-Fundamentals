namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Contracts;
    using Dtos;
    using Services.Contracts;

    public class RegisterUserCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly IUserSessionService userSessionService;

        public RegisterUserCommand(IUserService userService, IUserSessionService userSessionService)
        {
            this.userService = userService;
            this.userSessionService = userSessionService;
        }

        // RegisterUser <username> <password> <repeat-password> <email>
        public string Execute(string[] args)
        {
            if (args.Length < 4)
            {
                throw new ArgumentException("Command RegisterUser not valid!");
            }

            if (this.userSessionService.IsLoggedIn)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            string username = args[0];
            string password = args[1];
            string repeatPassword = args[2];
            string email = args[3];

            var registerUserDto = new RegisterUserDto
            {
                Username = username,
                Password = password,
                Email = email
            };

            if (!this.IsValid(registerUserDto))
            {
                throw new ArgumentException("Invalid data!");
            }

            bool userExists = this.userService.Exists(username);

            if (userExists)
            {
                throw new InvalidOperationException($"Username {username} is already taken!");
            }

            if (password != repeatPassword)
            {
                throw new ArgumentException("Passwords do not match!");
            }

            this.userService.Register(username, password, email);

            return $"User {username} was registered successfully!";
        }

        private bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}
