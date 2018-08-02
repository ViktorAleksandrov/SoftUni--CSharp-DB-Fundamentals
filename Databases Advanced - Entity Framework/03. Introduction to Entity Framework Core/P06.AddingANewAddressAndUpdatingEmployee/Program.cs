using System.IO;
using System.Linq;

using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace P06.AddingANewAddressAndUpdatingEmployee
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                var address = new Address
                {
                    AddressText = "Vitoshka 15",
                    TownId = 4
                };

                Employee employee = context.Employees
                    .FirstOrDefault(e => e.LastName == "Nakov");

                employee.Address = address;

                context.SaveChanges();

                string[] employeesAddresses = context.Employees
                    .OrderByDescending(e => e.AddressId)
                    .Take(10)
                    .Select(e => e.Address.AddressText)
                    .ToArray();

                using (var sw = new StreamWriter("../../../output.txt"))
                {
                    foreach (string empAddress in employeesAddresses)
                    {
                        sw.WriteLine(empAddress);
                    }
                }
            }
        }
    }
}
