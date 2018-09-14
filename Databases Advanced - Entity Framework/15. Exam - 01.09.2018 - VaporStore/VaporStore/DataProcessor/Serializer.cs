namespace VaporStore.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Data.Models.Enums;
    using Dto.Export;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var genres = context.Genres
                .Where(ge => genreNames.Contains(ge.Name))
                .Select(ge => new
                {
                    ge.Id,
                    Genre = ge.Name,
                    Games = ge.Games
                        .Where(ga => ga.Purchases.Any())
                        .Select(ga => new
                        {
                            ga.Id,
                            Title = ga.Name,
                            Developer = ga.Developer.Name,
                            Tags = string.Join(", ", ga.GameTags.Select(gt => gt.Tag.Name)),
                            Players = ga.Purchases.Count
                        })
                        .OrderByDescending(ga => ga.Players)
                        .ThenBy(ga => ga.Id)
                        .ToArray(),
                    TotalPlayers = ge.Games
                        .Where(ga => ga.Purchases.Any())
                        .Sum(ga => ga.Purchases.Count)
                })
                .OrderByDescending(ge => ge.TotalPlayers)
                .ThenBy(ge => ge.Id)
                .ToArray();

            string json = JsonConvert.SerializeObject(genres, Formatting.Indented);

            return json;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            PurchaseType type = Enum.Parse<PurchaseType>(storeType);

            UserExportDto[] users = context.Users
                .Select(u => new UserExportDto
                {
                    Username = u.Username,
                    Purchases = u.Cards
                        .SelectMany(c => c.Purchases)
                        .Where(p => p.Type == type)
                        .Select(p => new PurchaseExportDto
                        {
                            Card = p.Card.Number,
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Game = new GameExportDto
                            {
                                Title = p.Game.Name,
                                Genre = p.Game.Genre.Name,
                                Price = p.Game.Price
                            }
                        })
                        .OrderBy(p => p.Date)
                        .ToArray(),
                    TotalSpent = u.Cards
                        .SelectMany(c => c.Purchases)
                        .Where(p => p.Type == type)
                        .Sum(p => p.Game.Price)
                })
                .Where(ud => ud.Purchases.Any())
                .OrderByDescending(ud => ud.TotalSpent)
                .ThenBy(ud => ud.Username)
                .ToArray();

            var serializer = new XmlSerializer(typeof(UserExportDto[]), new XmlRootAttribute("Users"));

            var sb = new StringBuilder();
            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), users, xmlNamespaces);

            return sb.ToString();
        }
    }
}