using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using P2.VillainNames;

namespace P7.PrintAllMinionNames
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                string selectAllMinions = "SELECT Name FROM Minions";

                using (var command = new SqlCommand(selectAllMinions, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        var minions = new List<string>();

                        while (reader.Read())
                        {
                            minions.Add((string)reader["Name"]);
                        }

                        int count = minions.Count / 2;

                        for (int index = 0; index < count; index++)
                        {
                            Console.WriteLine(minions[index]);
                            Console.WriteLine(minions[minions.Count - 1 - index]);
                        }

                        if (minions.Count % 2 == 1)
                        {
                            Console.WriteLine(minions[count]);
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}
