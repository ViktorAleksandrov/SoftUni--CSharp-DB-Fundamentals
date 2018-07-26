using System;
using System.Data.SqlClient;
using P2.VillainNames;

namespace P6.RemoveVillain
{
    class Program
    {
        static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());

            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();

                string villainName = GetVillainName(villainId, connection, transaction);

                try
                {
                    if (villainName == null)
                    {
                        Console.WriteLine("No such villain was found.");
                    }
                    else
                    {
                        int releasedMinionsCount = ReleaseMinionsFromVillain(villainId, connection, transaction);

                        DeleteVillain(villainId, connection, transaction);

                        Console.WriteLine($"{villainName} was deleted.");
                        Console.WriteLine($"{releasedMinionsCount} minions were released.");

                        transaction.Commit();
                    }
                }
                catch
                {
                    transaction.Rollback();
                }

                connection.Close();
            }
        }

        private static void DeleteVillain(int villainId, SqlConnection connection, SqlTransaction transaction)
        {
            string deleteFromVillains = "DELETE FROM Villains WHERE Id = @villainId";

            using (var command = new SqlCommand(deleteFromVillains, connection, transaction))
            {
                command.Parameters.AddWithValue("@villainId", villainId);

                command.ExecuteNonQuery();
            }
        }

        private static int ReleaseMinionsFromVillain(int villainId, SqlConnection connection, SqlTransaction transaction)
        {
            string deleteFromMinionsVillains = "DELETE FROM MinionsVillains WHERE VillainId = @villainId";

            using (var command = new SqlCommand(deleteFromMinionsVillains, connection, transaction))
            {
                command.Parameters.AddWithValue("@villainId", villainId);

                int releasedMinionsCount = command.ExecuteNonQuery();

                return releasedMinionsCount;
            }
        }

        private static string GetVillainName(int villainId, SqlConnection connection, SqlTransaction transaction)
        {
            string selectVillainName = "SELECT Name FROM Villains WHERE Id = @villainId";

            using (var command = new SqlCommand(selectVillainName, connection, transaction))
            {
                command.Parameters.AddWithValue("@villainId", villainId);

                string villainName = (string)command.ExecuteScalar();

                return villainName;
            }
        }
    }
}
