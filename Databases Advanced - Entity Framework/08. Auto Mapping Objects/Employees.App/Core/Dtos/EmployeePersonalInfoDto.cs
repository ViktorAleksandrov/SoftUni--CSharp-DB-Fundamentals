namespace Employees.App.Core.Dtos
{
    using System;

    public class EmployeePersonalInfoDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public DateTime? Bitrhday { get; set; }

        public string Address { get; set; }
    }
}
