using System;
using System.IO;
using System.Linq;

using P02_DatabaseFirst.Data;

namespace P09.Employee147
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                var employee = context.Employees
                    .Where(e => e.EmployeeId == 147)
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle,
                        ProjectsNames = e.EmployeesProjects
                            .Select(ep => ep.Project.Name)
                            .OrderBy(pn => pn)
                            .ToArray()
                    })
                    .First();

                using (var sw = new StreamWriter("../../../output.txt"))
                {
                    sw.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

                    sw.WriteLine(string.Join(Environment.NewLine, employee.ProjectsNames));
                }
            }
        }
    }
}
