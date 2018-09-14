namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using Dto.Import;
    using Newtonsoft.Json;

    public static class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            GameDto[] deserializedGames = JsonConvert.DeserializeObject<GameDto[]>(jsonString);

            var sb = new StringBuilder();

            var developers = new List<Developer>();
            var genres = new List<Genre>();
            var tags = new List<Tag>();
            var games = new List<Game>();

            foreach (GameDto gameDto in deserializedGames)
            {
                if (!IsValid(gameDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Developer developer = developers.SingleOrDefault(d => d.Name == gameDto.Developer);

                if (developer == null)
                {
                    developer = new Developer { Name = gameDto.Developer };

                    developers.Add(developer);
                }

                Genre genre = genres.SingleOrDefault(g => g.Name == gameDto.Genre);

                if (genre == null)
                {
                    genre = new Genre { Name = gameDto.Genre };

                    genres.Add(genre);
                }

                var currentTags = new List<Tag>();

                foreach (string tagName in gameDto.Tags)
                {
                    Tag tag = tags.SingleOrDefault(t => t.Name == tagName);

                    if (tag == null)
                    {
                        tag = new Tag { Name = tagName };

                        tags.Add(tag);
                    }

                    currentTags.Add(tag);
                }

                var game = new Game
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = DateTime.ParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Developer = developer,
                    Genre = genre,
                    GameTags = currentTags.Select(t => new GameTag { Tag = t }).ToArray()
                };

                games.Add(game);

                sb.AppendLine($"Added {game.Name} ({genre.Name}) with {currentTags.Count} tags");
            }

            context.Games.AddRange(games);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            UserDto[] deserializedUsers = JsonConvert.DeserializeObject<UserDto[]>(jsonString);

            var sb = new StringBuilder();

            var users = new List<User>();

            foreach (UserDto userDto in deserializedUsers)
            {
                if (!IsValid(userDto) || userDto.Cards.Any(c => !IsValid(c)))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var user = new User
                {
                    Username = userDto.Username,
                    FullName = userDto.FullName,
                    Email = userDto.Email,
                    Age = userDto.Age,
                    Cards = userDto.Cards
                        .Select(c => new Card
                        {
                            Number = c.Number,
                            Cvc = c.CVC,
                            Type = Enum.Parse<CardType>(c.Type)
                        })
                        .ToArray()
                };

                users.Add(user);

                sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
            }

            context.Users.AddRange(users);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(PurchaseDto[]), new XmlRootAttribute("Purchases"));

            var deserializedPurchases = (PurchaseDto[])serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();

            var purchases = new List<Purchase>();

            foreach (PurchaseDto purchaseDto in deserializedPurchases)
            {
                if (!IsValid(purchaseDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Card card = context.Cards.Single(c => c.Number == purchaseDto.Card);
                Game game = context.Games.Single(g => g.Name == purchaseDto.Title);

                var purchase = new Purchase
                {
                    Type = Enum.Parse<PurchaseType>(purchaseDto.Type),
                    ProductKey = purchaseDto.Key,
                    Date = DateTime.ParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    Card = card,
                    Game = game,
                };

                purchases.Add(purchase);

                sb.AppendLine($"Imported {game.Name} for {card.User.Username}");
            }

            context.Purchases.AddRange(purchases);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);

            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}