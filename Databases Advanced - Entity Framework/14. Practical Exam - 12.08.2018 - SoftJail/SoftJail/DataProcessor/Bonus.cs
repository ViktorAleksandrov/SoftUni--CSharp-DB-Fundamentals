namespace SoftJail.DataProcessor
{
    using System;
    using System.Linq;
    using Data;
    using Data.Models;

    public class Bonus
    {
        public static string ReleasePrisoner(SoftJailDbContext context, int prisonerId)
        {
            Prisoner prisoner = context.Prisoners.SingleOrDefault(p => p.Id == prisonerId);

            if (prisoner.ReleaseDate == null)
            {
                return $"Prisoner {prisoner.FullName} is sentenced to life";
            }

            prisoner.ReleaseDate = DateTime.Now;
            prisoner.CellId = null;

            context.SaveChanges();

            return $"Prisoner {prisoner.FullName} released";
        }
    }
}
