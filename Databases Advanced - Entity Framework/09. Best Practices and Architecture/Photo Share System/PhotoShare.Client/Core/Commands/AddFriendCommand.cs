namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;

    using Contracts;
    using Dtos;
    using Services.Contracts;

    public class AddFriendCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly IUserSessionService userSessionService;

        public AddFriendCommand(IUserService userService, IUserSessionService userSessionService)
        {
            this.userService = userService;
            this.userSessionService = userSessionService;
        }

        // AddFriend <username1> <username2>
        public string Execute(string[] args)
        {
            if (args.Length < 2)
            {
                throw new ArgumentException("Command AddFriend not valid!");
            }

            string username = args[0];
            string friendUsername = args[1];

            bool userExists = this.userService.Exists(username);

            if (!userExists)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            bool friendExists = this.userService.Exists(friendUsername);

            if (!friendExists)
            {
                throw new ArgumentException($"User {friendUsername} not found!");
            }

            if (!this.userSessionService.IsLoggedIn || this.userSessionService.GetUsername != username)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            UserFriendsDto user = this.userService.ByUsername<UserFriendsDto>(username);
            UserFriendsDto friend = this.userService.ByUsername<UserFriendsDto>(friendUsername);

            bool isRequestSentFromUser = user.Friends.Any(f => f.Username == friendUsername);
            bool isRequestSentFromFriend = friend.Friends.Any(f => f.Username == username);

            if (isRequestSentFromUser && isRequestSentFromFriend)
            {
                throw new InvalidOperationException($"{friendUsername} is already a friend to {username}");
            }
            else if (isRequestSentFromUser && !isRequestSentFromFriend
                    || !isRequestSentFromUser && isRequestSentFromFriend)
            {
                throw new InvalidOperationException("Request is already sent");
            }

            this.userService.AddFriend(user.Id, friend.Id);

            return $"Friend {friendUsername} added to {username}!";
        }
    }
}
