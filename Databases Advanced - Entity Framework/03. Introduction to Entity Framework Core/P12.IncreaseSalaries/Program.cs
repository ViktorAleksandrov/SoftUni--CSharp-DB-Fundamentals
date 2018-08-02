using System.IO;
using System.Linq;

using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace P12.IncreaseSalaries
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                string[] departments = { "Engineering", "Tool Design", "Marketing", "Information Services" };

                Employee[] employees = context.Employees
                    .Where(e => departments.Contains(e.Department.Name))
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToArray();

                using (var sw = new StreamWriter("../../../output.txt"))
                {
                    foreach (Employee employee in employees)
                    {
                        employee.Salary *= 1.12m;

                        sw.WriteLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
