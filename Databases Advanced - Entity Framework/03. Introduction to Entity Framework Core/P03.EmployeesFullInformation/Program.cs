using System.IO;
using System.Linq;

using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace P03.EmployeesFullInformation
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                Employee[] employees = context.Employees
                    .OrderBy(e => e.EmployeeId)
                    .Select(e => new Employee
                    {
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        MiddleName = e.MiddleName,
                        JobTitle = e.JobTitle,
                        Salary = e.Salary
                    })
                    .ToArray();

                using (var sw = new StreamWriter("../../../employees.txt"))
                {
                    foreach (Employee emp in employees)
                    {
                        sw.WriteLine($"{emp.FirstName} {emp.LastName} {emp.MiddleName} {emp.JobTitle} {emp.Salary:F2}");
                    }
                }
            }
        }
    }
}
