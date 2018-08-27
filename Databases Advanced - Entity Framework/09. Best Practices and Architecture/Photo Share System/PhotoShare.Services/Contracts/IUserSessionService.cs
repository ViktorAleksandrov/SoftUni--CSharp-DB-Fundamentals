namespace PhotoShare.Services.Contracts
{
    using Models;

    public interface IUserSessionService
    {
        User User { get; }

        string GetUsername { get; }

        bool IsLoggedIn { get; }

        void Login(string username);

        void Logout();
    }
}
