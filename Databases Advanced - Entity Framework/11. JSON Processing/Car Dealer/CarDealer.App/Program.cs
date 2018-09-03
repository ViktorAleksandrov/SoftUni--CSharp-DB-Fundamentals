using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer.App
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new CarDealerContext())
            {
                ImportSuppliers(context);
                ImportParts(context);
                ImportCars(context);
                GeneratePartCars(context);
                ImportCustomers(context);
                GenerateSales(context);

                ExportOrderedCustomers(context);
                ExportCarsFromMakeToyota(context);
                ExportLocalSuppliers(context);
                ExportCarsWithTheirParts(context);
                ExportTotalSalesByCustomer(context);
                ExportSalesWithDiscount(context);
            }
        }

        private static void ExportSalesWithDiscount(CarDealerContext context)
        {
            var sales = context.Sales
               .Select(s => new
               {
                   car = new
                   {
                       s.Car.Make,
                       s.Car.Model,
                       s.Car.TravelledDistance
                   },
                   customerName = s.Customer.Name,
                   s.Discount,
                   price = s.Car.PartCars.Sum(pc => pc.Part.Price),
                   priceWithDiscount = s.Car.PartCars.Sum(pc => pc.Part.Price)
                        * (1 - (s.Customer.IsYoungDriver ? s.Discount + 0.05m : s.Discount))
               })
               .ToArray();

            string jsonString = JsonConvert.SerializeObject(sales, Formatting.Indented);

            File.WriteAllText("../../../Json/Export/sales-discounts.json", jsonString);
        }

        private static void ExportTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count > 0)
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count,
                    spentMoney = c.Sales.Select(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(customers, Formatting.Indented);

            File.WriteAllText("../../../Json/Export/customers-total-sales.json", jsonString);
        }

        private static void ExportCarsWithTheirParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TravelledDistance
                    },
                    parts = c.PartCars
                        .Select(pc => new
                        {
                            pc.Part.Name,
                            pc.Part.Price
                        })
                        .ToArray()
                })
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(cars, Formatting.Indented);

            File.WriteAllText("../../../Json/Export/cars-and-parts.json", jsonString);
        }

        private static void ExportLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            File.WriteAllText("../../../Json/Export/local-suppliers.json", jsonString);
        }

        private static void ExportCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    c.Model,
                    c.TravelledDistance
                })
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(cars, Formatting.Indented);

            File.WriteAllText("../../../Json/Export/toyota-cars.json", jsonString);
        }

        private static void ExportOrderedCustomers(CarDealerContext context)
        {
            Customer[] customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => !c.IsYoungDriver)
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(customers, Formatting.Indented);

            File.WriteAllText("../../../Json/Export/ordered-customers.json", jsonString);
        }

        private static void GenerateSales(CarDealerContext context)
        {
            var random = new Random();

            int[] carIds = context.Cars.Select(c => c.Id).ToArray();
            int[] customerIds = context.Customers.Select(c => c.Id).ToArray();

            decimal[] discounts = { 0, 0.05m, 0.1m, 0.15m, 0.2m, 0.3m, 0.4m, 0.5m };

            var sales = new List<Sale>();

            for (int count = 0; count < carIds.Length; count++)
            {
                int carId = carIds[random.Next(0, carIds.Length)];
                int customerId = customerIds[random.Next(0, customerIds.Length)];
                decimal discount = discounts[random.Next(0, discounts.Length)];

                var sale = new Sale
                {
                    CarId = carId,
                    CustomerId = customerId,
                    Discount = discount
                };

                sales.Add(sale);
            }

            context.Sales.AddRange(sales);

            context.SaveChanges();
        }

        private static void ImportCustomers(CarDealerContext context)
        {
            string jsonString = File.ReadAllText("../../../Json/Import/customers.json");

            Customer[] customers = JsonConvert.DeserializeObject<Customer[]>(jsonString);

            context.Customers.AddRange(customers);

            context.SaveChanges();
        }

        private static void GeneratePartCars(CarDealerContext context)
        {
            var random = new Random();

            int[] carIds = context.Cars.Select(c => c.Id).ToArray();
            int[] partIds = context.Parts.Select(p => p.Id).ToArray();

            var partCars = new List<PartCar>();

            foreach (int carId in carIds)
            {
                int partsPerCar = random.Next(10, 21);

                var randomPartIds = new List<int>();

                for (int i = 0; i < partsPerCar; i++)
                {
                    int partId = partIds[random.Next(0, partIds.Length)];

                    if (randomPartIds.Contains(partId))
                    {
                        i--;

                        continue;
                    }

                    randomPartIds.Add(partId);

                    var partCar = new PartCar { PartId = partId, CarId = carId };

                    partCars.Add(partCar);
                }
            }

            context.PartCars.AddRange(partCars);

            context.SaveChanges();
        }

        private static void ImportCars(CarDealerContext context)
        {
            string jsonString = File.ReadAllText("../../../Json/Import/cars.json");

            Car[] cars = JsonConvert.DeserializeObject<Car[]>(jsonString);

            context.Cars.AddRange(cars);

            context.SaveChanges();
        }

        private static void ImportParts(CarDealerContext context)
        {
            string jsonString = File.ReadAllText("../../../Json/Import/parts.json");

            Part[] parts = JsonConvert.DeserializeObject<Part[]>(jsonString);

            int[] supplierIds = context.Suppliers.Select(s => s.Id).ToArray();

            foreach (Part part in parts)
            {
                int supplierId = supplierIds[new Random().Next(0, supplierIds.Length)];

                part.SupplierId = supplierId;
            }

            context.Parts.AddRange(parts);

            context.SaveChanges();
        }

        private static void ImportSuppliers(CarDealerContext context)
        {
            string jsonString = File.ReadAllText("../../../Json/Import/suppliers.json");

            Supplier[] suppliers = JsonConvert.DeserializeObject<Supplier[]>(jsonString);

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();
        }
    }
}
