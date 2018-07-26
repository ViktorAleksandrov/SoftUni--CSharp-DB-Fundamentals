using System;
using System.Data.SqlClient;
using P2.VillainNames;

namespace P4.AddMinion
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] minionInfo = Console.ReadLine().Split();

            string minionName = minionInfo[1];
            int age = int.Parse(minionInfo[2]);
            string townName = minionInfo[3];

            string villainName = Console.ReadLine().Split()[1];

            using (var connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                int townId = GetTownId(townName, connection);

                int villainId = GetVillainId(villainName, connection);

                InsertIntoMinions(minionName, age, townId, connection);

                int minionId = GetMinionId(minionName, connection);

                AssignMinionToVillain(villainId, minionId, connection);

                Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");

                connection.Close();
            }
        }

        private static int GetTownId(string townName, SqlConnection connection)
        {
            string selectTownId = "SELECT Id FROM Towns WHERE Name = @townName";

            using (var command = new SqlCommand(selectTownId, connection))
            {
                command.Parameters.AddWithValue("@townName", townName);

                if (command.ExecuteScalar() == null)
                {
                    InsertIntoTowns(townName, connection);
                }

                int townId = (int)command.ExecuteScalar();

                return townId;
            }
        }

        private static void InsertIntoTowns(string townName, SqlConnection connection)
        {
            string insertTown = "INSERT INTO Towns (Name) VALUES (@townName)";

            using (var command = new SqlCommand(insertTown, connection))
            {
                command.Parameters.AddWithValue("@townName", townName);

                if (command.ExecuteNonQuery() == 1)
                {
                    Console.WriteLine($"Town {townName} was added to the database.");
                }
            }
        }

        private static int GetVillainId(string villainName, SqlConnection connection)
        {
            string selectVillainId = "SELECT Id FROM Villains WHERE Name = @villainName";

            using (var command = new SqlCommand(selectVillainId, connection))
            {
                command.Parameters.AddWithValue("@villainName", villainName);

                if (command.ExecuteScalar() == null)
                {
                    InsertIntoVillains(villainName, connection);
                }

                int villainId = (int)command.ExecuteScalar();

                return villainId;
            }
        }

        private static void InsertIntoVillains(string villainName, SqlConnection connection)
        {
            string insertVillain = "INSERT INTO Villains (Name, EvilnessFactorId) VALUES (@villainName, (SELECT Id FROM EvilnessFactors WHERE Name = 'Evil'))";

            using (var command = new SqlCommand(insertVillain, connection))
            {
                command.Parameters.AddWithValue("@villainName", villainName);

                if (command.ExecuteNonQuery() == 1)
                {
                    Console.WriteLine($"Villain {villainName} was added to the database.");
                }
            }
        }

        private static void InsertIntoMinions(string minionName, int age, int townId, SqlConnection connection)
        {
            string insertMinion = "INSERT INTO Minions(Name, Age, TownId) VALUES (@minionName, @age, @townId)";

            using (var command = new SqlCommand(insertMinion, connection))
            {
                command.Parameters.AddWithValue("@minionName", minionName);
                command.Parameters.AddWithValue("@age", age);
                command.Parameters.AddWithValue("@townId", townId);

                command.ExecuteNonQuery();
            }
        }

        private static int GetMinionId(string minionName, SqlConnection connection)
        {
            string selectMinionId = "SELECT Id FROM Minions WHERE Name = @minionName";

            using (var command = new SqlCommand(selectMinionId, connection))
            {
                command.Parameters.AddWithValue("@minionName", minionName);

                int minionId = (int)command.ExecuteScalar();

                return minionId;
            }
        }

        private static void AssignMinionToVillain(int villainId, int minionId, SqlConnection connection)
        {
            string insertMinionVillain = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";

            using (var command = new SqlCommand(insertMinionVillain, connection))
            {
                command.Parameters.AddWithValue("@minionId", minionId);
                command.Parameters.AddWithValue("@villainId", villainId);

                command.ExecuteNonQuery();
            }
        }
    }
}
