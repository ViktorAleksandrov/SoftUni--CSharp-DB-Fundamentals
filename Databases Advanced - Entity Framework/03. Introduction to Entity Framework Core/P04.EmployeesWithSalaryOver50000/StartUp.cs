using System.IO;
using System.Linq;

using P02_DatabaseFirst.Data;

namespace P04.EmployeesWithSalaryOver50000
{
    class StartUp
    {
        static void Main(string[] args)
        {
            using (var dbContext = new SoftUniContext())
            {
                string[] employeesNames = dbContext.Employees
                    .Where(e => e.Salary > 50_000)
                    .Select(e => e.FirstName)
                    .OrderBy(e => e)
                    .ToArray();

                using (var sw = new StreamWriter("../../../output.txt"))
                {
                    foreach (string name in employeesNames)
                    {
                        sw.WriteLine(name);
                    }
                }
            }
        }
    }
}
