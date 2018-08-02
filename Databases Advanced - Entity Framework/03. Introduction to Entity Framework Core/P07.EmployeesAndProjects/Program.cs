using System.Globalization;
using System.IO;
using System.Linq;

using P02_DatabaseFirst.Data;

namespace P07.EmployeesAndProjects
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                var employeesProjects = context.Employees
                    .Where(e => e.EmployeesProjects
                        .Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                    .Take(30)
                    .Select(e => new
                    {
                        EmployeeName = $"{e.FirstName} {e.LastName}",
                        ManagerName = $"{e.Manager.FirstName} {e.Manager.LastName}",
                        Projects = e.EmployeesProjects
                            .Select(ep => new
                            {
                                ep.Project.Name,
                                ep.Project.StartDate,
                                ep.Project.EndDate
                            })
                    })
                    .ToArray();

                using (var sw = new StreamWriter("../../../output.txt"))
                {
                    foreach (var ep in employeesProjects)
                    {
                        sw.WriteLine($"{ep.EmployeeName} - Manager: {ep.ManagerName}");

                        foreach (var project in ep.Projects)
                        {
                            string dateFormat = "M/d/yyyy h:mm:ss tt";

                            sw.WriteLine(
                                $"--{project.Name} - " +
                                $"{project.StartDate.ToString(dateFormat, CultureInfo.InvariantCulture)} - " +
                                $"{project.EndDate?.ToString(dateFormat, CultureInfo.InvariantCulture) ?? "not finished"}");
                        }
                    }
                }
            }
        }
    }
}
