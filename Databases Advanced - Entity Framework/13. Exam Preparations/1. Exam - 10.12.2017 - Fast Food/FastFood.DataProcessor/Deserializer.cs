using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Import;
using FastFood.Models;
using FastFood.Models.Enums;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
    public static class Deserializer
    {
        private const string FailureMessage = "Invalid data format.";
        private const string SuccessMessage = "Record {0} successfully imported.";

        public static string ImportEmployees(FastFoodDbContext context, string jsonString)
        {
            EmployeeDto[] deserializedEmployees = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString);

            var sb = new StringBuilder();

            var employees = new List<Employee>();

            foreach (EmployeeDto employeeDto in deserializedEmployees)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Position position = context.Positions.SingleOrDefault(p => p.Name == employeeDto.Position);

                if (position == null)
                {
                    position = new Position { Name = employeeDto.Position };

                    context.Positions.Add(position);

                    context.SaveChanges();
                }

                var employee = new Employee
                {
                    Name = employeeDto.Name,
                    Age = employeeDto.Age,
                    Position = position
                };

                employees.Add(employee);

                sb.AppendLine(string.Format(SuccessMessage, employee.Name));
            }

            context.Employees.AddRange(employees);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportItems(FastFoodDbContext context, string jsonString)
        {
            ItemDto[] deserializedItems = JsonConvert.DeserializeObject<ItemDto[]>(jsonString);

            var sb = new StringBuilder();

            var items = new List<Item>();

            foreach (ItemDto itemDto in deserializedItems)
            {
                if (!IsValid(itemDto) || items.Any(i => i.Name == itemDto.Name))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Category category = context.Categories.SingleOrDefault(c => c.Name == itemDto.Category);

                if (category == null)
                {
                    category = new Category { Name = itemDto.Category };

                    context.Categories.Add(category);

                    context.SaveChanges();
                }

                var item = new Item
                {
                    Name = itemDto.Name,
                    Price = itemDto.Price,
                    Category = category
                };

                items.Add(item);

                sb.AppendLine(string.Format(SuccessMessage, item.Name));
            }

            context.Items.AddRange(items);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOrders(FastFoodDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(OrderDto[]), new XmlRootAttribute("Orders"));

            var deserializedOrders = (OrderDto[])serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            var orders = new List<Order>();
            var orderItems = new List<OrderItem>();

            foreach (OrderDto orderDto in deserializedOrders)
            {
                bool isItemValid = true;

                if (!IsValid(orderDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                foreach (OrderItemDto orderItemDto in orderDto.Items)
                {
                    if (!IsValid(orderItemDto))
                    {
                        sb.AppendLine(FailureMessage);

                        isItemValid = false;
                        break;
                    }
                }

                if (!isItemValid)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Employee employee = context.Employees.SingleOrDefault(e => e.Name == orderDto.Employee);

                if (employee == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                bool itemsExist = CheckIfAllItemsExist(context, orderDto.Items);

                if (!itemsExist)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var datetime = DateTime.ParseExact(orderDto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                OrderType type = Enum.TryParse(orderDto.Type, out OrderType orderType) ? orderType : OrderType.ForHere;

                var order = new Order
                {
                    Customer = orderDto.Customer,
                    DateTime = datetime,
                    Type = type,
                    Employee = employee
                };

                orders.Add(order);

                foreach (OrderItemDto orderItemDto in orderDto.Items)
                {
                    Item item = context.Items.SingleOrDefault(i => i.Name == orderItemDto.Name);

                    var orderItem = new OrderItem
                    {
                        Order = order,
                        Item = item,
                        Quantity = orderItemDto.Quantity
                    };

                    orderItems.Add(orderItem);
                }

                string date = datetime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                sb.AppendLine($"Order for {order.Customer} on {date} added");
            }

            context.Orders.AddRange(orders);
            context.SaveChanges();

            context.OrderItems.AddRange(orderItems);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool CheckIfAllItemsExist(FastFoodDbContext context, OrderItemDto[] items)
        {
            foreach (OrderItemDto item in items)
            {
                bool itemExist = context.Items.Any(i => i.Name == item.Name);

                if (!itemExist)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);

            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}