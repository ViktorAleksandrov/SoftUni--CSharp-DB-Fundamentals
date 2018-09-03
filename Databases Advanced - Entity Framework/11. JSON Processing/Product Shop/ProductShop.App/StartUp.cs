namespace ProductShop.App
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;

    using Data;
    using Models;

    using Newtonsoft.Json;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new ProductShopContext())
            {
                ImportUsers(context);
                ImportProducts(context);
                ImportCategories(context);
                GenerateCategoryProducts(context);

                ExportProductsInRange(context);
                ExportSuccessfullySoldProducts(context);
                ExportCategoriesByProductsCount(context);
                ExportUsersAndProducts(context);
            }
        }

        private static void ExportUsersAndProducts(ProductShopContext context)
        {
            var usersWithProducts = new
            {
                usersCount = context.Users.Count(u => u.ProductsSold.Any(p => p.Buyer != null)),
                users = context.Users
                    .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                    .OrderByDescending(u => u.ProductsSold.Count)
                    .ThenBy(u => u.LastName)
                    .Select(u => new
                    {
                        firstName = u.FirstName,
                        lastName = u.LastName,
                        age = u.Age == null ? 0 : u.Age,
                        soldProducts = new
                        {
                            count = u.ProductsSold.Count,
                            products = u.ProductsSold
                                .Select(p => new
                                {
                                    name = p.Name,
                                    price = p.Price
                                })
                                .ToArray()
                        }
                    })
                    .ToArray()
            };

            string jsonString = JsonConvert.SerializeObject(usersWithProducts, Formatting.Indented);

            File.WriteAllText("../../../Json/Export/users-and-products.json", jsonString);
        }

        private static void ExportCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count,
                    averagePrice = c.CategoryProducts.Sum(cp => cp.Product.Price) / c.CategoryProducts.Count,
                    totalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                })
                .OrderByDescending(c => c.productsCount)
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(categories, Formatting.Indented);

            File.WriteAllText("../../../Json/Export/categories-by-products.json", jsonString);
        }

        private static void ExportSuccessfullySoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldproducts = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price,
                            buyerFirstName = p.Buyer.FirstName,
                            buyerLastName = p.Buyer.LastName
                        })
                        .ToArray()
                })
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(users, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            File.WriteAllText("../../../Json/Export/users-sold-products.json", jsonString);
        }

        private static void ExportProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = $"{p.Seller.FirstName} {p.Seller.LastName}".TrimStart()
                })
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(products, Formatting.Indented);

            File.WriteAllText("../../../Json/Export/products-in-range.json", jsonString);
        }

        private static void GenerateCategoryProducts(ProductShopContext context)
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

        private static void ImportCategories(ProductShopContext context)
        {
            string jsonString = File.ReadAllText("../../../Json/Import/categories.json");

            Category[] deserializedCategories = JsonConvert.DeserializeObject<Category[]>(jsonString);

            var categories = new List<Category>();

            foreach (Category category in deserializedCategories)
            {
                if (IsValid(category))
                {
                    categories.Add(category);
                }
            }

            context.Categories.AddRange(categories);

            context.SaveChanges();
        }

        private static void ImportProducts(ProductShopContext context)
        {
            string jsonString = File.ReadAllText("../../../Json/Import/products.json");

            Product[] deserializedProducts = JsonConvert.DeserializeObject<Product[]>(jsonString);

            var random = new Random();

            int usersCount = context.Users.Count();
            int middleOfUsersCount = 1 + (usersCount / 2);

            var products = new List<Product>();

            foreach (Product product in deserializedProducts)
            {
                if (!IsValid(product))
                {
                    continue;
                }

                int sellerId = random.Next(1, middleOfUsersCount);

                product.SellerId = sellerId;

                if (random.Next(1, 5) != 3)
                {
                    int buyerId = random.Next(middleOfUsersCount, usersCount + 1);

                    product.BuyerId = buyerId;
                }

                products.Add(product);
            }

            context.Products.AddRange(products);

            context.SaveChanges();
        }

        private static void ImportUsers(ProductShopContext context)
        {
            string jsonString = File.ReadAllText("../../../Json/Import/users.json");

            User[] deserializedUsers = JsonConvert.DeserializeObject<User[]>(jsonString);

            var users = new List<User>();

            foreach (User user in deserializedUsers)
            {
                if (IsValid(user))
                {
                    users.Add(user);
                }
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
