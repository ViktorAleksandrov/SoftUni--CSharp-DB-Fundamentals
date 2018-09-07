using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class ShowEventCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(1, inputArgs);

            string eventName = inputArgs[0];

            if (!CommandHelper.IsEventExisting(eventName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound, eventName));
            }

            Event @event;

            using (var context = new TeamBuilderContext())
            {
                @event = context.Events
                    .Include(e => e.ParticipatingEventTeams)
                    .ThenInclude(et => et.Team)
                    .Where(e => e.Name == eventName)
                    .OrderByDescending(et => et.StartDate)
                    .First();
            }

            var sb = new StringBuilder();

            string eventStartDate = @event.StartDate.ToString(Constants.DateTimeFormat, CultureInfo.InvariantCulture);
            string eventEndDate = @event.EndDate.ToString(Constants.DateTimeFormat, CultureInfo.InvariantCulture);

            sb.AppendLine($"{@event.Name} {eventStartDate} {eventEndDate}");
            sb.AppendLine(@event.Description);
            sb.AppendLine("Teams:");

            foreach (EventTeam eventTeam in @event.ParticipatingEventTeams)
            {
                sb.AppendLine($"-{eventTeam.Team.Name}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
