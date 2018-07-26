using System.Data.SqlClient;

namespace P1.InitialSetup
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Server=VIKTOR-PC\SQLEXPRESS;Integrated Security=True";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string createDb = "CREATE DATABASE MinionsDB";
                ExecuteNonQueryCommand(createDb, connection);

                connection.ChangeDatabase("MinionsDB");

                string createTableCountries = "CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50))";
                ExecuteNonQueryCommand(createTableCountries, connection);

                string createTableTowns = "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50), CountryCode INT FOREIGN KEY REFERENCES Countries(Id))";
                ExecuteNonQueryCommand(createTableTowns, connection);

                string createTableMinions = "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(30), Age INT, TownId INT FOREIGN KEY REFERENCES Towns(Id))";
                ExecuteNonQueryCommand(createTableMinions, connection);

                string createTableEvilnessFactors = "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))";
                ExecuteNonQueryCommand(createTableEvilnessFactors, connection);

                string createTableVillains = "CREATE TABLE Villains (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))";
                ExecuteNonQueryCommand(createTableVillains, connection);

                string createTableMinionsVillains = "CREATE TABLE MinionsVillains (MinionId INT FOREIGN KEY REFERENCES Minions(Id),VillainId INT FOREIGN KEY REFERENCES Villains(Id),CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId))";
                ExecuteNonQueryCommand(createTableMinionsVillains, connection);

                string insertIntoCountries = "INSERT INTO Countries ([Name]) VALUES ('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')";
                ExecuteNonQueryCommand(insertIntoCountries, connection);

                string insertIntoTowns = "INSERT INTO Towns ([Name], CountryCode) VALUES ('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 4)";
                ExecuteNonQueryCommand(insertIntoTowns, connection);

                string insertIntoMinions = "INSERT INTO Minions (Name,Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)";
                ExecuteNonQueryCommand(insertIntoMinions, connection);

                string insertIntoEvilnessFactors = "INSERT INTO EvilnessFactors (Name) VALUES ('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')";
                ExecuteNonQueryCommand(insertIntoEvilnessFactors, connection);

                string insertIntoVillains = "INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('Gru',2),('Victor',1),('Jilly',3),('Miro',4),('Rosen',5),('Dimityr',1),('Dobromir',2)";
                ExecuteNonQueryCommand(insertIntoVillains, connection);

                string insertIntoMinionsVillains = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (4,2),(1,1),(5,7),(3,5),(2,6),(11,5),(8,4),(9,7),(7,1),(1,3),(7,3),(5,3),(4,3),(1,2),(2,1),(2,7)";
                ExecuteNonQueryCommand(insertIntoMinionsVillains, connection);

                connection.Close();
            }
        }

        private static void ExecuteNonQueryCommand(string command, SqlConnection connection)
        {
            using (var sqlCommand = new SqlCommand(command, connection))
            {
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}
