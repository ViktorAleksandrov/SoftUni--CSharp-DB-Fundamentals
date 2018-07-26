using System;
using System.Data.SqlClient;
using P2.VillainNames;

namespace P9.IncreaseAgeStoredProcedure
{
    class Program
    {
        static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());

            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                string execProc = "EXEC usp_GetOlder @id";

                using (var command = new SqlCommand(execProc,connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }

                string selectMinion = "SELECT Name, Age FROM Minions WHERE Id = @id";

                using (var command = new SqlCommand(selectMinion, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            string name = (string)reader["Name"];
                            int age = (int)reader["Age"];

                            Console.WriteLine($"{name} – {age} years old");
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}
