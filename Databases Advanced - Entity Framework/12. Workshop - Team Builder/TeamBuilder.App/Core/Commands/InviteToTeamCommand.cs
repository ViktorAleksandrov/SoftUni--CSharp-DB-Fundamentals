using System;
using System.Linq;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class InviteToTeamCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(2, inputArgs);

            AuthenticationManager.Authorize();

            string teamName = inputArgs[0];
            string username = inputArgs[1];

            bool teamExist = CommandHelper.IsTeamExisting(teamName);
            bool userExist = CommandHelper.IsUserExisting(username);

            if (!teamExist || !userExist)
            {
                throw new ArgumentException(Constants.ErrorMessages.TeamOrUserNotExist);
            }

            User invitedUser;

            using (var context = new TeamBuilderContext())
            {
                invitedUser = context.Users.Single(u => u.Username == username);
            }

            if (CommandHelper.IsInviteExisting(teamName, invitedUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.InviteIsAlreadySent);
            }

            User currentUser = AuthenticationManager.GetCurrentUser();

            bool isCurrentUserCreator = CommandHelper.IsUserCreatorOfTeam(teamName, currentUser);
            bool isCurrentUserInTeam = CommandHelper.IsMemberOfTeam(teamName, currentUser.Username);
            bool isInvitedUserInTeam = CommandHelper.IsMemberOfTeam(teamName, username);

            if ((!isCurrentUserCreator && !isCurrentUserInTeam) || isInvitedUserInTeam)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            bool isInvitedUserCreator = CommandHelper.IsUserCreatorOfTeam(teamName, invitedUser);

            int teamId;

            using (var context = new TeamBuilderContext())
            {
                teamId = context.Teams.Single(t => t.Name == teamName).Id;
            }

            if (isInvitedUserCreator)
            {
                var userTeam = new UserTeam
                {
                    UserId = invitedUser.Id,
                    TeamId = teamId
                };

                using (var context = new TeamBuilderContext())
                {
                    context.UserTeams.Add(userTeam);

                    context.SaveChanges();
                }
            }
            else
            {
                var invitation = new Invitation
                {
                    InvitedUserId = invitedUser.Id,
                    TeamId = teamId
                };

                using (var context = new TeamBuilderContext())
                {
                    context.Invitations.Add(invitation);

                    context.SaveChanges();
                }
            }

            return $"Team {teamName} invited {username}!";
        }
    }
}
