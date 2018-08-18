namespace Employees.App.Core.Dtos
{
    using System.Collections.Generic;

    public class ManagerDto
    {
        public ManagerDto()
        {
            this.EmployeeDtos = new HashSet<EmployeeDto>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int EmployeesCount => this.EmployeeDtos.Count;

        public ICollection<EmployeeDto> EmployeeDtos { get; set; }
    }
}
