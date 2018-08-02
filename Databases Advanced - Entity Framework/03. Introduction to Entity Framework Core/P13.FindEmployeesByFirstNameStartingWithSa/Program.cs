using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using P02_DatabaseFirst.Data;

namespace P13.FindEmployeesByFirstNameStartingWithSa
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                var employees = context.Employees
                    .Where(e => EF.Functions.Like(e.FirstName, "Sa%"))
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle,
                        e.Salary
                    })
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToArray();

                using (var sw = new StreamWriter("../../../output.txt"))
                {
                    foreach (var emp in employees)
                    {
                        sw.WriteLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle} - (${emp.Salary:F2})");
                    }
                }
            }
        }
    }
}
