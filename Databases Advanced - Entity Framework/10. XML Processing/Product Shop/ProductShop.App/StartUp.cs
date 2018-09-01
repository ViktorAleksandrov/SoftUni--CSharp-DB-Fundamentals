using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using ProductShop.App.Dto.Export;
using ProductShop.App.Dto.Import;
using ProductShop.Data;
using ProductShop.Models;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace ProductShop.App
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            IMapper mapper = mapperConfig.CreateMapper();

            using (var context = new ProductShopContext())
            {
                ImportUsers(context, mapper);
                ImportProducts(context, mapper);
                ImportCategories(context, mapper);
                ImportCategoryProducts(context);

                ExportProductsInRange(context);
                ExportSoldProducts(context);
                ExportCategoriesByProductsCount(context);
                ExportUsersAndProducts(context);
            }
        }

        private static void ExportUsersAndProducts(ProductShopContext context)
        {
            var users = new UsersDto
            {
                UsersCount = context.Users.Count(),
                Users = context.Users
                    .Where(u => u.SoldProducts.Count > 0)
                    .OrderByDescending(u => u.SoldProducts.Count)
                    .ThenBy(u => u.LastName)
                    .Select(u => new UserInfoDto
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age.ToString(),
                        SoldProducts = new ProductInfoDto
                        {
                            ProductsCount = u.SoldProducts.Count,
                            SoldProducts = u.SoldProducts
                                .Select(pr => new SoldProductDto
                                {
                                    Name = pr.Name,
                                    Price = pr.Price
                                })
                                .ToArray()
                        }
                    })
                    .ToArray()
            };

            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(UsersDto), new XmlRootAttribute("users"));

            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), users, xmlNamespaces);

            File.WriteAllText("../../../Xml/Export/users-and-products.xml", sb.ToString());
        }

        private static void ExportCategoriesByProductsCount(ProductShopContext context)
        {
            CategoryExportDto[] categories = context.Categories
                .OrderByDescending(c => c.CategoryProducts.Count)
                .Select(c => new CategoryExportDto
                {
                    Name = c.Name,
                    ProductsCount = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts
                        .Select(cp => cp.Product.Price)
                        .DefaultIfEmpty(0)
                        .Average(),
                    TotalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .ToArray();

            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(CategoryExportDto[]), new XmlRootAttribute("categories"));

            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), categories, xmlNamespaces);

            File.WriteAllText("../../../Xml/Export/categories-by-products.xml", sb.ToString());
        }

        private static void ExportSoldProducts(ProductShopContext context)
        {
            UserExportDto[] users = context.Users
                .Where(u => u.SoldProducts.Count > 0)
                .Select(u => new UserExportDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.SoldProducts
                        .Select(p => new ProductDto
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .ToArray()
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToArray();

            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(UserExportDto[]), new XmlRootAttribute("users"));

            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), users, xmlNamespaces);

            File.WriteAllText("../../../Xml/Export/users-sold-products.xml", sb.ToString());
        }

        private static void ExportProductsInRange(ProductShopContext context)
        {
            ProductExportDto[] products = context.Products
                .Where(p => p.Price >= 1000 && p.Price <= 2000 && p.Buyer != null)
                .OrderBy(p => p.Price)
                .Select(p => new ProductExportDto
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = p.Buyer.FirstName + " " + p.Buyer.LastName ?? p.Buyer.LastName
                })
                .ToArray();

            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ProductExportDto[]), new XmlRootAttribute("products"));

            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), products, xmlNamespaces);

            File.WriteAllText("../../../Xml/Export/products-in-range.xml", sb.ToString());
        }

        private static void ImportCategoryProducts(ProductShopContext context)
        {
            int[] productsIds = context.Products
                .Select(p => p.Id)
                .ToArray();

            int categoriesCount = context.Categories.Count();

            var categoryProducts = new List<CategoryProduct>();

            foreach (int productId in productsIds)
            {
                int categoryId = new Random().Next(1, categoriesCount + 1);

                var categoryProduct = new CategoryProduct
                {
                    CategoryId = categoryId,
                    ProductId = productId
                };

                categoryProducts.Add(categoryProduct);
            }

            context.CategoryProducts.AddRange(categoryProducts);

            context.SaveChanges();
        }

        private static void ImportCategories(ProductShopContext context, IMapper mapper)
        {
            string xmlString = File.ReadAllText("../../../Xml/Import/categories.xml");

            var serializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("categories"));

            var deserializedCategories = (CategoryDto[])serializer.Deserialize(new StringReader(xmlString));

            var categories = new List<Category>();

            foreach (CategoryDto categoryDto in deserializedCategories)
            {
                if (!IsValid(categoryDto))
                {
                    continue;
                }

                Category category = mapper.Map<Category>(categoryDto);

                categories.Add(category);
            }

            context.Categories.AddRange(categories);

            context.SaveChanges();
        }

        private static void ImportProducts(ProductShopContext context, IMapper mapper)
        {
            string xmlString = File.ReadAllText("../../../Xml/Import/products.xml");

            var serializer = new XmlSerializer(typeof(ProductDto[]), new XmlRootAttribute("products"));

            var deserializedProducts = (ProductDto[])serializer.Deserialize(new StringReader(xmlString));

            var products = new List<Product>();

            int counter = 1;

            foreach (ProductDto productDto in deserializedProducts)
            {
                if (!IsValid(productDto))
                {
                    continue;
                }

                Product product = mapper.Map<Product>(productDto);

                int buyerId = new Random().Next(1, 29);
                int sellerId = new Random().Next(29, 57);

                product.BuyerId = buyerId;
                product.SellerId = sellerId;

                if (counter % 4 == 0)
                {
                    product.BuyerId = null;
                }

                products.Add(product);

                counter++;
            }

            context.Products.AddRange(products);

            context.SaveChanges();
        }

        private static void ImportUsers(ProductShopContext context, IMapper mapper)
        {
            string xmlString = File.ReadAllText("../../../Xml/Import/users.xml");

            var serializer = new XmlSerializer(typeof(UserDto[]), new XmlRootAttribute("users"));

            var deserializedUsers = (UserDto[])serializer.Deserialize(new StringReader(xmlString));

            var users = new List<User>();

            foreach (UserDto userDto in deserializedUsers)
            {
                if (!IsValid(userDto))
                {
                    continue;
                }

                User user = mapper.Map<User>(userDto);

                users.Add(user);
            }

            context.Users.AddRange(users);

            context.SaveChanges();
        }

        public static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);

            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}
