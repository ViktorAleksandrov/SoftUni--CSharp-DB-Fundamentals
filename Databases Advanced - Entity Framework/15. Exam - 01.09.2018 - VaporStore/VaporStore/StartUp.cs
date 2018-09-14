namespace VaporStore
{
    using System;
    using System.IO;
    using AutoMapper;
    using Data;
    using DataProcessor;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new VaporStoreDbContext();

            Mapper.Initialize(config => config.AddProfile<VaporStoreProfile>());

            ResetDatabase(context, shouldDropDatabase: true);

            string projectDir = GetProjectDirectory();

            ImportEntities(context, projectDir + @"Datasets/", projectDir + @"ImportResults/");
            ExportEntities(context, projectDir + @"ImportResults/");

            using (var transaction = context.Database.BeginTransaction())
            {
                BonusTask(context);
                transaction.Rollback();
            }
        }

        private static void BonusTask(VaporStoreDbContext context)
        {
            string bonusOutput = Bonus.UpdateEmail(context, "atobin", "amontobin@gmail.com");
            Console.WriteLine(bonusOutput);
        }

        private static void ExportEntities(VaporStoreDbContext context, string exportDir)
        {
            string jsonOutput = Serializer.ExportGamesByGenres(context, new[] { "Nudity", "Violent" });
            PrintAndExportEntityToFile(jsonOutput, exportDir + "GamesByGenres.json");

            string xmlOutput = Serializer.ExportUserPurchasesByType(context, "Digital");
            PrintAndExportEntityToFile(xmlOutput, exportDir + "UserPurchases.xml");
        }

        private static void ImportEntities(VaporStoreDbContext context, string baseDir, string exportDir)
        {
            string games = Deserializer.ImportGames(context, File.ReadAllText(baseDir + "games.json"));
            PrintAndExportEntityToFile(games, exportDir + "ImportGames.txt");

            string users = Deserializer.ImportUsers(context, File.ReadAllText(baseDir + "users.json"));
            PrintAndExportEntityToFile(users, exportDir + "ImportUsers.txt");

            string purchases = Deserializer.ImportPurchases(context, File.ReadAllText(baseDir + "purchases.xml"));
            PrintAndExportEntityToFile(purchases, exportDir + "ImportPurchases.txt");
        }

        private static void ResetDatabase(DbContext context, bool shouldDropDatabase = false)
        {
            if (shouldDropDatabase)
            {
                context.Database.EnsureDeleted();
            }

            if (context.Database.EnsureCreated())
            {
                return;
            }

            string disableIntegrityChecksQuery = "EXEC sp_MSforeachtable @command1='ALTER TABLE ? NOCHECK CONSTRAINT ALL'";
            context.Database.ExecuteSqlCommand(disableIntegrityChecksQuery);

            string deleteRowsQuery = "EXEC sp_MSforeachtable @command1='SET QUOTED_IDENTIFIER ON;DELETE FROM ?'";
            context.Database.ExecuteSqlCommand(deleteRowsQuery);

            string enableIntegrityChecksQuery =
             "EXEC sp_MSforeachtable @command1='ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'";
            context.Database.ExecuteSqlCommand(enableIntegrityChecksQuery);

            string reseedQuery =
             "EXEC sp_MSforeachtable @command1='IF OBJECT_ID(''?'') IN (SELECT OBJECT_ID FROM SYS.IDENTITY_COLUMNS) DBCC CHECKIDENT(''?'', RESEED, 0)'";
            context.Database.ExecuteSqlCommand(reseedQuery);
        }

        private static void PrintAndExportEntityToFile(string entityOutput, string outputPath)
        {
            Console.WriteLine(entityOutput);
            File.WriteAllText(outputPath, entityOutput.TrimEnd());
        }

        private static string GetProjectDirectory()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string directoryName = Path.GetFileName(currentDirectory);
            string relativePath = directoryName.StartsWith("netcoreapp") ? @"../../../" : string.Empty;

            return relativePath;
        }
    }
}