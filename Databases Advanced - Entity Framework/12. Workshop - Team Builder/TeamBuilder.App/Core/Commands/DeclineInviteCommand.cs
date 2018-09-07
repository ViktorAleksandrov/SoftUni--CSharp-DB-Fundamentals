using System;
using System.Linq;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class DeclineInviteCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(1, inputArgs);

            AuthenticationManager.Authorize();

            string teamName = inputArgs[0];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            User currentUser = AuthenticationManager.GetCurrentUser();

            if (!CommandHelper.IsInviteExisting(teamName, currentUser))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InviteNotFound, teamName));
            }

            this.DeclineInvite(currentUser, teamName);

            return $"Invite from {teamName} declined.";
        }

        private void DeclineInvite(User currentUser, string teamName)
        {
            using (var context = new TeamBuilderContext())
            {
                int teamId = context.Teams.Single(t => t.Name == teamName).Id;

                context.Invitations
                    .Single(i => i.InvitedUserId == currentUser.Id && i.TeamId == teamId)
                    .IsActive = false;

                context.SaveChanges();
            }
        }
    }
}
