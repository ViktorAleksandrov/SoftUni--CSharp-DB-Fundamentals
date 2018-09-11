using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Export;
using FastFood.Models.Enums;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
    public class Serializer
    {
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {
            OrderType type = Enum.Parse<OrderType>(orderType);

            var employee = context.Employees
                .ToArray()
                .Where(e => e.Name == employeeName)
                .Select(e => new
                {
                    e.Name,
                    Orders = e.Orders
                        .Where(o => o.Type == type)
                        .Select(o => new
                        {
                            o.Customer,
                            Items = o.OrderItems
                                .Select(oi => new
                                {
                                    oi.Item.Name,
                                    oi.Item.Price,
                                    oi.Quantity
                                })
                                .ToArray(),
                            o.TotalPrice
                        })
                        .OrderByDescending(o => o.TotalPrice)
                        .ThenByDescending(o => o.Items.Length)
                        .ToArray(),
                    TotalMade = e.Orders
                        .Where(o => o.Type == type)
                        .Sum(o => o.TotalPrice)
                })
                .SingleOrDefault();

            string json = JsonConvert.SerializeObject(employee, Newtonsoft.Json.Formatting.Indented);

            return json;
        }

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
        {
            string[] categoryNames = categoriesString.Split(',');

            CategoryDto[] categories = context.Categories
                .Where(c => categoryNames.Any(s => s == c.Name))
                .Select(c => new CategoryDto
                {
                    Name = c.Name,
                    MostPopularItem = c.Items
                        .Select(i => new CategoryItemDto
                        {
                            Name = i.Name,
                            TotalMade = i.OrderItems.Sum(oi => oi.Item.Price * oi.Quantity),
                            TimesSold = i.OrderItems.Sum(oi => oi.Quantity)
                        })
                        .OrderByDescending(i => i.TotalMade)
                        .ThenByDescending(i => i.TimesSold)
                        .FirstOrDefault()
                })
                .OrderByDescending(c => c.MostPopularItem.TotalMade)
                .ThenByDescending(c => c.MostPopularItem.TimesSold)
                .ToArray();

            var serializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("Categories"));

            var sb = new StringBuilder();
            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), categories, xmlNamespaces);

            return sb.ToString();
        }
    }
}