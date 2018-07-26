using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using P2.VillainNames;

namespace P5.ChangeTownNamesCasing
{
    class Program
    {
        static void Main(string[] args)
        {
            string countryName = Console.ReadLine();

            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                int countryId = GetCountryId(countryName, connection);

                if (countryId == 0)
                {
                    Console.WriteLine("No town names were affected.");
                }
                else
                {
                    int townsCount = UpdateTownNames(countryId, connection);

                    Console.WriteLine($"{townsCount} town names were affected.");

                    PrintTownNames(countryId, connection);
                }

                connection.Close();
            }
        }

        private static void PrintTownNames(int countryId, SqlConnection connection)
        {
            string selectTowns = "SELECT Name FROM Towns WHERE CountryCode = @countryId";

            using (var command = new SqlCommand(selectTowns, connection))
            {
                command.Parameters.AddWithValue("@countryId", countryId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var towns = new List<string>();

                    while (reader.Read())
                    {
                        string town = (string)reader["Name"];

                        towns.Add(town);
                    }

                    Console.WriteLine($"[{string.Join(", ", towns)}]");
                }
            }
        }

        private static int UpdateTownNames(int countryId, SqlConnection connection)
        {
            string updateTowns = "UPDATE Towns SET Name = UPPER(Name) WHERE CountryCode = @countryId";

            using (var command = new SqlCommand(updateTowns, connection))
            {
                command.Parameters.AddWithValue("@countryId", countryId);

                int townsCount = command.ExecuteNonQuery();

                return townsCount;
            }
        }

        private static int GetCountryId(string countryName, SqlConnection connection)
        {
            string selectCountryId = "SELECT TOP (1) c.Id FROM Towns AS t JOIN Countries AS c ON c.Id = t.CountryCode WHERE c.Name = @countryName";

            using (var command = new SqlCommand(selectCountryId, connection))
            {
                command.Parameters.AddWithValue("@countryName", countryName);

                if (command.ExecuteScalar() == null)
                {
                    return 0;
                }

                int countryId = (int)command.ExecuteScalar();

                return countryId;
            }
        }
    }
}
