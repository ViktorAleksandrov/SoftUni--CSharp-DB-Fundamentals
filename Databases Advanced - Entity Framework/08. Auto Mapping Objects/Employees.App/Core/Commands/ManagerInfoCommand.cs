namespace Employees.App.Core.Commands
{
    using System.Text;

    using Contracts;
    using Dtos;

    public class ManagerInfoCommand : ICommand
    {
        private readonly IManagerController managerController;

        public ManagerInfoCommand(IManagerController managerController)
        {
            this.managerController = managerController;
        }

        public string Execute(string[] args)
        {
            int employeeId = int.Parse(args[0]);

            ManagerDto manager = this.managerController.GetManagerInfo(employeeId);

            var sb = new StringBuilder();

            sb.AppendLine($"{manager.FirstName} {manager.LastName} | Employees: {manager.EmployeesCount}");

            foreach (EmployeeDto employee in manager.EmployeeDtos)
            {
                sb.AppendLine($"    - {employee.FirstName} {employee.LastName} - ${employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
