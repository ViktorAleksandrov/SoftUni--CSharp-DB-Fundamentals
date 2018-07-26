using System;
using System.Data.SqlClient;

namespace P2.VillainNames
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                string villainMinionsCount = "SELECT v.Name, COUNT(mv.MinionId) AS MinionsCount FROM Villains AS v JOIN MinionsVillains AS mv ON mv.VillainId = v.Id GROUP BY v.Name HAVING COUNT(mv.MinionId) > 3 ORDER BY MinionsCount DESC";

                using (var command = new SqlCommand(villainMinionsCount, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string villainName = reader["Name"].ToString();
                            int minionsCount = (int)reader["MinionsCount"];

                            Console.WriteLine($"{villainName} - {minionsCount}");
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}
