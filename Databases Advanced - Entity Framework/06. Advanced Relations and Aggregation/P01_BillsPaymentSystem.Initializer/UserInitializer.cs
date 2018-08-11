using P01_BillsPaymentSystem.Data.Models;

namespace P01_BillsPaymentSystem.Initializer
{
    public class UserInitializer
    {
        public static User[] GetUsers()
        {
            User[] users =
            {
                new User { FirstName = "Pesho", LastName = "Petrov", Email = "pesho@abv.bg", Password = "pass1" },
                new User { FirstName = "Gosho", LastName = "Georgiev", Email = "gosho@abv.bg", Password = "pass2" }
            };

            return users;
        }
    }
}