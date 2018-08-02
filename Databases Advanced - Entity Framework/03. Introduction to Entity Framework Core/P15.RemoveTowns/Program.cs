using System;
using System.Linq;

using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace P15.RemoveTowns
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                string townName = Console.ReadLine();

                Employee[] employees = context.Employees
                    .Where(e => e.Address.Town.Name == townName)
                    .ToArray();

                foreach (Employee emp in employees)
                {
                    emp.AddressId = null;
                }

                Address[] addresses = context.Addresses
                    .Where(a => a.Town.Name == townName)
                    .ToArray();

                Town town = context.Towns
                    .SingleOrDefault(t => t.Name == townName);

                context.Addresses.RemoveRange(addresses);

                context.Towns.Remove(town);

                context.SaveChanges();

                int addressesCount = addresses.Length;

                Console.WriteLine(
                    $"{addressesCount} {(addressesCount == 1 ? "address" : "addresses")}" +
                    $" in {townName} {(addressesCount == 1 ? "was" : "were")} deleted");
            }
        }
    }
}
