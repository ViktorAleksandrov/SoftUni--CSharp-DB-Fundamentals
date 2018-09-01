namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using System.Text;

    using Contracts;
    using Dtos;
    using Services.Contracts;

    public class PrintFriendsListCommand : ICommand
    {
        private readonly IUserService userService;

        public PrintFriendsListCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentException("Command PrintFriendsList not valid!");
            }

            string username = args[0];

            bool userExists = this.userService.Exists(username);

            if (!userExists)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            UserFriendsDto user = this.userService.ByUsername<UserFriendsDto>(username);

            var sb = new StringBuilder();

            if (user.Friends.Count == 0)
            {
                sb.AppendLine("No friends for this user. :(");
            }
            else
            {
                sb.AppendLine("Friends:");

                foreach (FriendDto friend in user.Friends.OrderBy(f => f.Username))
                {
                    sb.AppendLine($"-{friend.Username}");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
