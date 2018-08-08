using P03_FootballBetting.Data;

namespace P03_FootballBetting
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new FootballBettingContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
    }
}
