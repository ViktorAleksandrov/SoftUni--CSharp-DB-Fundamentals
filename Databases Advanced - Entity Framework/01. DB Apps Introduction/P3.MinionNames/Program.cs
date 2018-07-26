using System;
using System.Data.SqlClient;
using P2.VillainNames;

namespace P3.MinionNames
{
    class Program
    {
        static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());

            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                string villainName = GetVillainName(villainId, connection);

                if (villainName == null)
                {
                    Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                    return;
                }

                Console.WriteLine($"Villain: {villainName}");

                PrintMinionsNames(villainId, connection);

                connection.Close();
            }
        }

        private static void PrintMinionsNames(int villainId, SqlConnection connection)
        {
            string minionsQuery = $"SELECT m.Name, m.Age FROM Minions AS m JOIN MinionsVillains AS mv ON mv.MinionId = m.Id WHERE mv.VillainId = @id";

            using (var command = new SqlCommand(minionsQuery, connection))
            {
                command.Parameters.AddWithValue("@id", villainId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("(no minions)");
                        return;
                    }

                    int row = 1;

                    while (reader.Read())
                    {
                        string minionName = (string)reader["Name"];
                        int minionAge = (int)reader["Age"];

                        Console.WriteLine($"{row++}. {minionName} {minionAge}");
                    }
                }
            }
        }

        private static string GetVillainName(int villainId, SqlConnection connection)
        {
            string villainNameQuery = $"SELECT Name FROM Villains WHERE Id = @id";

            using (var command = new SqlCommand(villainNameQuery, connection))
            {
                command.Parameters.AddWithValue("@id", villainId);

                return (string)command.ExecuteScalar();
            }
        }
    }
}
