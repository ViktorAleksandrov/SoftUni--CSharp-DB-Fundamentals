namespace Employees.App.Core.Commands
{
    using System;

    using Contracts;
    using Dtos;

    public class EmployeePersonalInfoCommand : ICommand
    {
        private readonly IEmployeeController employeeController;

        public EmployeePersonalInfoCommand(IEmployeeController employeeController)
        {
            this.employeeController = employeeController;
        }

        public string Execute(string[] args)
        {
            int employeeId = int.Parse(args[0]);

            EmployeePersonalInfoDto emp = this.employeeController.GetEmployeePersonalInfo(employeeId);

            return $"ID: {emp.Id} - {emp.FirstName} {emp.LastName} - ${emp.Salary:f2}{Environment.NewLine}" +
                $"Birthday: {emp.Bitrhday.Value.ToString("dd-MM-yyy")}{Environment.NewLine}" +
                $"Address: {emp.Address}";
        }
    }
}
