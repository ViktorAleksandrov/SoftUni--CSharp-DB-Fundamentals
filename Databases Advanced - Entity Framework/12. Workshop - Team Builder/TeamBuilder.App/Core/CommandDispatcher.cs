using System;
using System.Linq;
using TeamBuilder.App.Core.Commands;

namespace TeamBuilder.App.Core
{
    public class CommandDispatcher
    {
        public string Dispatch(string input)
        {
            string result = string.Empty;

            string[] inputArgs = input.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            string commandName = inputArgs.Length > 0 ? inputArgs[0] : string.Empty;

            inputArgs = inputArgs.Skip(1).ToArray();

            switch (commandName)
            {
                case "RegisterUser":
                    var registerUser = new RegisterUserCommand();
                    result = registerUser.Execute(inputArgs);
                    break;
                case "Login":
                    var login = new LoginCommand();
                    result = login.Execute(inputArgs);
                    break;
                case "Logout":
                    var logout = new LogoutCommand();
                    result = logout.Execute(inputArgs);
                    break;
                case "DeleteUser":
                    var deleteUser = new DeleteUserCommand();
                    result = deleteUser.Execute(inputArgs);
                    break;
                case "CreateEvent":
                    var createEvent = new CreateEventCommand();
                    result = createEvent.Execute(inputArgs);
                    break;
                case "CreateTeam":
                    var createTeam = new CreateTeamCommand();
                    result = createTeam.Execute(inputArgs);
                    break;
                case "InviteToTeam":
                    var inviteToTeam = new InviteToTeamCommand();
                    result = inviteToTeam.Execute(inputArgs);
                    break;
                case "AcceptInvite":
                    var acceptInvite = new AcceptInviteCommand();
                    result = acceptInvite.Execute(inputArgs);
                    break;
                case "DeclineInvite":
                    var declineInvite = new DeclineInviteCommand();
                    result = declineInvite.Execute(inputArgs);
                    break;
                case "KickMember":
                    var kickMember = new KickMemberCommand();
                    result = kickMember.Execute(inputArgs);
                    break;
                case "Disband":
                    var disband = new DisbandCommand();
                    result = disband.Execute(inputArgs);
                    break;
                case "AddTeamTo":
                    var addTeamTo = new AddTeamToCommand();
                    result = addTeamTo.Execute(inputArgs);
                    break;
                case "ShowEvent":
                    var showEvent = new ShowEventCommand();
                    result = showEvent.Execute(inputArgs);
                    break;
                case "ShowTeam":
                    var showTeam = new ShowTeamCommand();
                    result = showTeam.Execute(inputArgs);
                    break;
                case "Exit":
                    var exit = new ExitCommand();
                    exit.Execute(inputArgs);
                    break;
                default:
                    throw new NotSupportedException($"Command {commandName} not supported!");
            }

            return result;
        }
    }
}
