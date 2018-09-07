using System;
using System.Linq;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class KickMemberCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(2, inputArgs);

            AuthenticationManager.Authorize();

            string teamName = inputArgs[0];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            string username = inputArgs[1];

            if (!CommandHelper.IsUserExisting(username))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UserNotFound, username));
            }

            if (!CommandHelper.IsMemberOfTeam(teamName, username))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.NotPartOfTeam, username, teamName));
            }

            User currentUser = AuthenticationManager.GetCurrentUser();

            if (!CommandHelper.IsUserCreatorOfTeam(teamName, currentUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            if (currentUser.Username == username)
            {
                throw new InvalidOperationException(
                    string.Format(Constants.ErrorMessages.CommandNotAllowed, "DisbandTeam"));
            }

            this.KickMember(teamName, username);

            return $"User {username} was kicked from {teamName}!";
        }

        private void KickMember(string teamName, string username)
        {
            using (var context = new TeamBuilderContext())
            {
                int userId = context.Users.Single(u => u.Username == username).Id;
                int teamId = context.Teams.Single(t => t.Name == teamName).Id;

                UserTeam userTeam = context.UserTeams
                    .Single(ut => ut.UserId == userId && ut.TeamId == teamId);

                context.UserTeams.Remove(userTeam);

                context.SaveChanges();
            }
        }
    }
}
