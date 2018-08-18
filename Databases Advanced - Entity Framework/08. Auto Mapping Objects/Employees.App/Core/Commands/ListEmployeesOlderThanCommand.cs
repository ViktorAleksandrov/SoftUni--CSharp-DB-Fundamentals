namespace Employees.App.Core.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Contracts;
    using Dtos;

    public class ListEmployeesOlderThanCommand : ICommand
    {
        private readonly IEmployeeController employeeController;

        public ListEmployeesOlderThanCommand(IEmployeeController employeeController)
        {
            this.employeeController = employeeController;
        }

        public string Execute(string[] args)
        {
            int age = int.Parse(args[0]);

            IEnumerable<EmployeesOlderThanDto> employees = this.employeeController.ListEmployeesOlderThan(age)
                .OrderByDescending(e => e.Salary);

            var sb = new StringBuilder();

            if (!employees.Any())
            {
                sb.AppendLine($"No employees older than {age}");
            }
            else
            {
                foreach (EmployeesOlderThanDto emp in employees)
                {
                    sb.AppendLine($"{emp.FirstName} {emp.LastName} - ${emp.Salary:f2} - " +
                        $"Manager: {(emp.Manager != null ? emp.Manager.LastName : "[no manager]")}");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
