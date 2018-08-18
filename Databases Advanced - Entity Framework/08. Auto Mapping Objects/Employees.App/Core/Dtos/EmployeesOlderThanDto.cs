namespace Employees.App.Core.Dtos
{
    using Models;

    public class EmployeesOlderThanDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public Employee Manager { get; set; }
    }
}
