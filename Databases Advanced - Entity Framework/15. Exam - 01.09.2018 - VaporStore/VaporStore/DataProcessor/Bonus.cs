namespace VaporStore.DataProcessor
{
    using System.Linq;
    using Data;
    using Data.Models;

    public static class Bonus
    {
        public static string UpdateEmail(VaporStoreDbContext context, string username, string newEmail)
        {
            User user = context.Users.SingleOrDefault(u => u.Username == username);

            if (user == null)
            {
                return $"User {username} not found";
            }

            bool emailExists = context.Users.Any(u => u.Email == newEmail);

            if (emailExists)
            {
                return $"Email {newEmail} is already taken";
            }

            user.Email = newEmail;

            context.SaveChanges();

            return $"Changed {username}'s email successfully";
        }
    }
}
