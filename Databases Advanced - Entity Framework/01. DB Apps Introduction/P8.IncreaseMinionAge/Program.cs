using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using P2.VillainNames;

namespace P8.IncreaseMinionAge
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] minionsIds = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToArray();

            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                foreach (int id in minionsIds)
                {
                    string minionName = GetMinionName(id, connection);

                    if (minionName == null)
                    {
                        continue;
                    }

                    string titleMinionName = new CultureInfo("en-US", false)
                        .TextInfo
                        .ToTitleCase(minionName.ToLower());

                    UpdateMinions(titleMinionName, id, connection);
                }

                PrintMinions(connection);

                connection.Close();
            }
        }

        private static void PrintMinions(SqlConnection connection)
        {
            string selectMinion = "SELECT Name, Age FROM Minions";

            using (var command = new SqlCommand(selectMinion, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = (string)reader["Name"];
                        int age = (int)reader["Age"];

                        Console.WriteLine($"{name} {age}");
                    }
                }
            }
        }

        private static void UpdateMinions(string titleMinionName, int id, SqlConnection connection)
        {
            string updateMinion = "UPDATE Minions SET Age+= 1, Name = @titleMinionName WHERE Id = @minionId";

            using (var command = new SqlCommand(updateMinion, connection))
            {
                command.Parameters.AddWithValue("@titleMinionName", titleMinionName);
                command.Parameters.AddWithValue("@minionId", id);

                command.ExecuteNonQuery();
            }
        }

        private static string GetMinionName(int id, SqlConnection connection)
        {
            string selectMinionName = "SELECT Name FROM Minions WHERE Id = @minionId";

            using (var command = new SqlCommand(selectMinionName, connection))
            {
                command.Parameters.AddWithValue("@minionId", id);

                string minionName = (string)command.ExecuteScalar();

                return minionName;
            }
        }
    }
}
