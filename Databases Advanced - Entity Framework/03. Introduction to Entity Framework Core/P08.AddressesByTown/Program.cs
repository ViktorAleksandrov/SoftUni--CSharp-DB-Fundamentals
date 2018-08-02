using System.IO;
using System.Linq;

using P02_DatabaseFirst.Data;

namespace P08.AddressesByTown
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                var addresses = context.Addresses
                    .Select(a => new
                    {
                        a.AddressText,
                        TownName = a.Town.Name,
                        EmployeesCount = a.Employees.Count
                    })
                    .OrderByDescending(a => a.EmployeesCount)
                    .ThenBy(a => a.TownName)
                    .ThenBy(a => a.AddressText)
                    .Take(10)
                    .ToArray();

                using (var sw = new StreamWriter("../../../output.txt"))
                {
                    foreach (var address in addresses)
                    {
                        sw.WriteLine($"{address.AddressText}, {address.TownName} - {address.EmployeesCount} employees");
                    }
                }
            }
        }
    }
}
