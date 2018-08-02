using System.IO;
using System.Linq;

using P02_DatabaseFirst.Data;

namespace P05.EmployeesFromResearchAndDevelopment
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dbContext = new SoftUniContext())
            {
                var employees = dbContext.Employees
                    .Where(e => e.Department.Name == "Research and Development")
                    .OrderBy(e => e.Salary)
                    .ThenByDescending(e => e.FirstName)
                    .Select(e => new { e.FirstName, e.LastName, DepartmentName = e.Department.Name, e.Salary })
                    .ToArray();

                using (var sw = new StreamWriter("../../../output.txt"))
                {
                    foreach (var emp in employees)
                    {
                        sw.WriteLine($"{emp.FirstName} {emp.LastName} from {emp.DepartmentName} - ${emp.Salary:F2}");
                    }
                }
            }
        }
    }
}
