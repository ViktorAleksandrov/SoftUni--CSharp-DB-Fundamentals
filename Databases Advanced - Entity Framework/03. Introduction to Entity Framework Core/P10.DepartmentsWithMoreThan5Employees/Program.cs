using System.IO;
using System.Linq;

using P02_DatabaseFirst.Data;

namespace P10.DepartmentsWithMoreThan5Employees
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                var departments = context.Departments
                    .Where(d => d.Employees.Count > 5)
                    .OrderBy(d => d.Employees.Count)
                    .ThenBy(d => d.Name)
                    .Select(d => new
                    {
                        d.Name,
                        ManagerName = $"{d.Manager.FirstName} {d.Manager.LastName}",
                        Employees = d.Employees
                            .Select(e => new
                            {
                                e.FirstName,
                                e.LastName,
                                e.JobTitle
                            })
                            .OrderBy(e => e.FirstName)
                            .ThenBy(e => e.LastName)
                    })
                    .ToArray();

                using (var sw = new StreamWriter("../../../output.txt"))
                {
                    foreach (var department in departments)
                    {
                        sw.WriteLine($"{department.Name} - {department.ManagerName}");

                        foreach (var employee in department.Employees)
                        {
                            sw.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                        }

                        sw.WriteLine(new string('-', 10));
                    }
                }
            }
        }
    }
}
