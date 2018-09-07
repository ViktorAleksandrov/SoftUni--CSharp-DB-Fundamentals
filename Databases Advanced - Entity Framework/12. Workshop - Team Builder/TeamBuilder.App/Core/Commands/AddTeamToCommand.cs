using System;
using System.Linq;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class AddTeamToCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(2, inputArgs);

            AuthenticationManager.Authorize();

            string eventName = inputArgs[0];

            if (!CommandHelper.IsEventExisting(eventName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound, eventName));
            }

            string teamName = inputArgs[1];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            User currentUser = AuthenticationManager.GetCurrentUser();

            if (!CommandHelper.IsUserCreatorOfEvent(eventName, currentUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            using (var context = new TeamBuilderContext())
            {
                Event @event = context.Events
                    .Where(e => e.Name == eventName)
                    .OrderByDescending(et => et.StartDate)
                    .First();

                Team team = context.Teams
                    .Single(t => t.Name == teamName);

                bool eventTeamExist = context.EventTeams
                    .Any(et => et.EventId == @event.Id && et.TeamId == team.Id);

                if (eventTeamExist)
                {
                    throw new InvalidOperationException(Constants.ErrorMessages.CannotAddSameTeamTwice);
                }

                var eventTeam = new EventTeam
                {
                    Event = @event,
                    Team = team
                };

                context.EventTeams.Add(eventTeam);

                context.SaveChanges();
            }

            return $"Team {teamName} added for {eventName}!";
        }
    }
}
