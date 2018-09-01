using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using CarDealer.App.Dto.Export;
using CarDealer.App.Dto.Import;
using CarDealer.Data;
using CarDealer.Models;

namespace CarDealer.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());

            IMapper mapper = mapperConfig.CreateMapper();

            using (var context = new CarDealerContext())
            {
                ImportSuppliers(context, mapper);
                ImportParts(context, mapper);
                ImportCars(context, mapper);
                ImportPartCars(context);
                ImportCustomers(context, mapper);
                ImportSales(context);

                ExportCarsWithDistance(context);
                ExportCarsFromMakeFerrari(context);
                ExportLocalSuppliers(context);
                ExportCarsWithParts(context);
                ExportTotalSalesByCustomer(context);
                ExportSalesWithDiscount(context);
            }
        }

        private static void ExportSalesWithDiscount(CarDealerContext context)
        {
            SaleDto[] sales = context.Sales
                .Select(s => new SaleDto
                {
                    Car = new CarExportDto
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    CustomerName = s.Customer.Name,
                    Discount = decimal.Parse(s.Discount.ToString("G29")),
                    Price = decimal.Parse(s.Car.PartCars.Sum(pc => pc.Part.Price).ToString("G29")),
                    PriceWithDiscount = decimal.Parse((s.Car.PartCars.Sum(pc => pc.Part.Price) * (1 - (s.Customer.IsYoungDriver ? s.Discount + 0.05m : s.Discount))).ToString("G29"))
                })
                .ToArray();

            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(SaleDto[]), new XmlRootAttribute("sales"));

            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), sales, xmlNamespaces);

            File.WriteAllText("../../../Xml/Export/sales-discounts.xml", sb.ToString());
        }

        private static void ExportTotalSalesByCustomer(CarDealerContext context)
        {
            CustomerExportDto[] customers = context.Customers
                .Where(c => c.BoughtCars.Count > 0)
                .Select(c => new CustomerExportDto
                {
                    Name = c.Name,
                    BoughtCarsCount = c.BoughtCars.Count,
                    SpentMoney = Math.Round(
                        c.BoughtCars.Select(s => s.Car.PartCars.Sum(pc => pc.Part.Price)
                                * (1 - (c.IsYoungDriver ? s.Discount + 0.05m : s.Discount)))
                            .DefaultIfEmpty(0).Sum(),
                        2, MidpointRounding.AwayFromZero)
                })
                .OrderByDescending(c => c.SpentMoney)
                .ThenByDescending(c => c.BoughtCarsCount)
                .ToArray();

            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(CustomerExportDto[]), new XmlRootAttribute("customers"));

            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), customers, xmlNamespaces);

            File.WriteAllText("../../../Xml/Export/customers-total-sales.xml", sb.ToString());
        }

        private static void ExportCarsWithParts(CarDealerContext context)
        {
            CarPartDto[] cars = context.Cars
                .Select(c => new CarPartDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.PartCars
                        .Select(pc => new PartExportDto
                        {
                            Name = pc.Part.Name,
                            Price = pc.Part.Price
                        })
                        .ToArray()
                })
                .ToArray();

            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(CarPartDto[]), new XmlRootAttribute("cars"));

            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), cars, xmlNamespaces);

            File.WriteAllText("../../../Xml/Export/cars-and-parts.xml", sb.ToString());
        }

        private static void ExportLocalSuppliers(CarDealerContext context)
        {
            LocalSupplierDto[] suppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .Select(s => new LocalSupplierDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(LocalSupplierDto[]), new XmlRootAttribute("suppliers"));

            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), suppliers, xmlNamespaces);

            File.WriteAllText("../../../Xml/Export/local-suppliers.xml", sb.ToString());
        }

        private static void ExportCarsFromMakeFerrari(CarDealerContext context)
        {
            CarFerrariDto[] cars = context.Cars
                .Where(c => c.Make == "Ferrari")
                .Select(c => new CarFerrariDto
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(CarFerrariDto[]), new XmlRootAttribute("cars"));

            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), cars, xmlNamespaces);

            File.WriteAllText("../../../Xml/Export/ferrari-cars.xml", sb.ToString());
        }

        private static void ExportCarsWithDistance(CarDealerContext context)
        {
            CarDto[] cars = context.Cars
                .Where(c => c.TravelledDistance > 2_000_000)
                .Select(c => new CarDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .ToArray();

            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(CarDto[]), new XmlRootAttribute("cars"));

            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), cars, xmlNamespaces);

            File.WriteAllText("../../../Xml/Export/cars.xml", sb.ToString());
        }

        private static void ImportSales(CarDealerContext context)
        {
            var random = new Random();

            int carsCount = context.Cars.Count();
            int customersCount = context.Customers.Count();

            decimal[] discounts = { 0, 0.05m, 0.1m, 0.15m, 0.2m, 0.3m, 0.4m, 0.5m };

            var sales = new List<Sale>();

            for (int count = 0; count < carsCount; count++)
            {
                int carId = random.Next(1, carsCount + 1);
                int customerId = random.Next(1, customersCount + 1);

                int discountIndex = random.Next(0, discounts.Length);

                decimal discount = discounts[discountIndex];

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

        private static void ImportCustomers(CarDealerContext context, IMapper mapper)
        {
            string xmlString = File.ReadAllText("../../../Xml/Import/customers.xml");

            var serializer = new XmlSerializer(typeof(CustomerDto[]), new XmlRootAttribute("customers"));

            var deserializedCustomers = (CustomerDto[])serializer.Deserialize(new StringReader(xmlString));

            var customers = new List<Customer>();

            foreach (CustomerDto customerDto in deserializedCustomers)
            {
                Customer customer = mapper.Map<Customer>(customerDto);

                customers.Add(customer);
            }

            context.Customers.AddRange(customers);

            context.SaveChanges();
        }

        private static void ImportPartCars(CarDealerContext context)
        {
            int carsCount = context.Cars.Count();
            int partsCount = context.Parts.Count();

            var partCars = new List<PartCar>();

            for (int carId = 1; carId <= carsCount; carId++)
            {
                int partsPerCar = new Random().Next(10, 21);

                var partIds = new List<int>();

                for (int i = 0; i < partsPerCar; i++)
                {
                    int partId = new Random().Next(1, partsCount + 1);

                    if (partIds.Contains(partId))
                    {
                        i--;

                        continue;
                    }

                    partIds.Add(partId);

                    var partCar = new PartCar { PartId = partId, CarId = carId };

                    partCars.Add(partCar);
                }
            }

            context.PartCars.AddRange(partCars);

            context.SaveChanges();
        }

        private static void ImportCars(CarDealerContext context, IMapper mapper)
        {
            string xmlString = File.ReadAllText("../../../Xml/Import/cars.xml");

            var serializer = new XmlSerializer(typeof(CarDto[]), new XmlRootAttribute("cars"));

            var deserializedCars = (CarDto[])serializer.Deserialize(new StringReader(xmlString));

            var cars = new List<Car>();

            foreach (CarDto carDto in deserializedCars)
            {
                Car car = mapper.Map<Car>(carDto);

                cars.Add(car);
            }

            context.Cars.AddRange(cars);

            context.SaveChanges();
        }

        private static void ImportParts(CarDealerContext context, IMapper mapper)
        {
            string xmlString = File.ReadAllText("../../../Xml/Import/parts.xml");

            var serializer = new XmlSerializer(typeof(PartDto[]), new XmlRootAttribute("parts"));

            var deserializedParts = (PartDto[])serializer.Deserialize(new StringReader(xmlString));

            int suppliersCount = context.Suppliers.Count();

            var parts = new List<Part>();

            foreach (PartDto partDto in deserializedParts)
            {
                Part part = mapper.Map<Part>(partDto);

                int supplierId = new Random().Next(1, suppliersCount + 1);

                part.SupplierId = supplierId;

                parts.Add(part);
            }

            context.Parts.AddRange(parts);

            context.SaveChanges();
        }

        private static void ImportSuppliers(CarDealerContext context, IMapper mapper)
        {
            string xmlString = File.ReadAllText("../../../Xml/Import/suppliers.xml");

            var serializer = new XmlSerializer(typeof(SupplierImportDto[]), new XmlRootAttribute("suppliers"));

            var desializedSuppliers = (SupplierImportDto[])serializer.Deserialize(new StringReader(xmlString));

            var suppliers = new List<Supplier>();

            foreach (SupplierImportDto supplierDto in desializedSuppliers)
            {
                Supplier supplier = mapper.Map<Supplier>(supplierDto);

                suppliers.Add(supplier);
            }

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();
        }
    }
}
