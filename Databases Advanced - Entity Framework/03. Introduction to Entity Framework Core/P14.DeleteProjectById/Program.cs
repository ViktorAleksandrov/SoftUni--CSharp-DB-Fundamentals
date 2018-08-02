using System.IO;
using System.Linq;

using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace P14.DeleteProjectById
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                EmployeeProject[] projects = context.EmployeesProjects
                    .Where(ep => ep.ProjectId == 2)
                    .ToArray();

                context.EmployeesProjects.RemoveRange(projects);

                Project project = context.Projects.Find(2);

                context.Projects.Remove(project);

                context.SaveChanges();

                string[] projectsNames = context.Projects
                    .Take(10)
                    .Select(p => p.Name)
                    .ToArray();

                using (var sw = new StreamWriter("../../../output.txt"))
                {
                    foreach (string name in projectsNames)
                    {
                        sw.WriteLine(name);
                    }
                }
            }
        }
    }
}
