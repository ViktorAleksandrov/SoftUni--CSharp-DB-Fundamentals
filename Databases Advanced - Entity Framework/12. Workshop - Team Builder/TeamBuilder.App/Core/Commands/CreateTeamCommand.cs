using System;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class CreateTeamCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            if (inputArgs.Length != 2 && inputArgs.Length != 3)
            {
                throw new FormatException(Constants.ErrorMessages.InvalidArgumentsCount);
            }

            AuthenticationManager.Authorize();

            string name = inputArgs[0];

            if (CommandHelper.IsTeamExisting(name))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamExists, name));
            }

            string acronym = inputArgs[1];

            if (acronym.Length != 3)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InvalidAcronym, acronym));
            }

            string description = null;

            if (inputArgs.Length == 3)
            {
                description = inputArgs[2];
            }

            User currentUser = AuthenticationManager.GetCurrentUser();

            var team = new Team
            {
                Name = name,
                Description = description,
                Acronym = acronym,
                CreatorId = currentUser.Id
            };

            using (var context = new TeamBuilderContext())
            {
                context.Teams.Add(team);

                context.SaveChanges();
            }

            return $"Team {name} successfully created!";
        }
    }
}
