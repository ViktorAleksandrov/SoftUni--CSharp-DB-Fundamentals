using System;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class ShowTeamCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(1, inputArgs);

            string teamName = inputArgs[0];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            Team team;

            using (var context = new TeamBuilderContext())
            {
                team = context.Teams
                    .Include(t => t.UserTeams)
                    .ThenInclude(ut => ut.User)
                    .Single(t => t.Name == teamName);
            }

            var sb = new StringBuilder();

            sb.AppendLine($"{team.Name} {team.Acronym}");
            sb.AppendLine("Members:");

            foreach (UserTeam userTeam in team.UserTeams)
            {
                sb.AppendLine($"-{userTeam.User.Username}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
