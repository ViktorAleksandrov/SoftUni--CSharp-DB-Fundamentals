using System;
using System.Globalization;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class CreateEventCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(6, inputArgs);

            AuthenticationManager.Authorize();

            string name = inputArgs[0];
            string description = inputArgs[1];

            string startDateString = $"{inputArgs[2]} {inputArgs[3]}";
            DateTime startDate = this.ParseDate(startDateString);

            string endDateString = $"{inputArgs[4]} {inputArgs[5]}";
            DateTime endDate = this.ParseDate(endDateString);

            if (startDate > endDate)
            {
                throw new ArgumentException("Start date should be before end date.");
            }

            User currentUser = AuthenticationManager.GetCurrentUser();

            var @event = new Event
            {
                Name = name,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
                CreatorId = currentUser.Id
            };

            using (var context = new TeamBuilderContext())
            {
                context.Events.Add(@event);

                context.SaveChanges();
            }

            return $"Event {name} was created successfully!";
        }

        private DateTime ParseDate(string dateString)
        {
            bool IsDateValid = DateTime.TryParseExact(
                dateString,
                Constants.DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime date);

            if (!IsDateValid)
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidDateFormat);
            }

            return date;
        }
    }
}
