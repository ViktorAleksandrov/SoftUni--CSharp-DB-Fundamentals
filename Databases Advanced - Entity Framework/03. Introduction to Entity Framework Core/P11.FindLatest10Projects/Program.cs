using System.Globalization;
using System.IO;
using System.Linq;

using P02_DatabaseFirst.Data;

namespace P11.FindLatest10Projects
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                var projects = context.Projects
                    .OrderByDescending(p => p.StartDate)
                    .Take(10)
                    .Select(p => new
                    {
                        p.Name,
                        p.Description,
                        StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                    })
                    .OrderBy(p => p.Name)
                    .ToArray();

                using (var sw = new StreamWriter("../../../output.txt"))
                {
                    foreach (var project in projects)
                    {
                        sw.WriteLine(project.Name);
                        sw.WriteLine(project.Description);
                        sw.WriteLine(project.StartDate);
                    }
                }
            }
        }
    }
}
