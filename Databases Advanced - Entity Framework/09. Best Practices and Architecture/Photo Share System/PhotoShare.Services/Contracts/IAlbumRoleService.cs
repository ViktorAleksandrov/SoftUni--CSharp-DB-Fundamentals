namespace PhotoShare.Services.Contracts
{
    using Models;

    public interface IAlbumRoleService
    {
        AlbumRole PublishAlbumRole(int albumId, int userId, string role);

        bool UserOwnsAlbum(string username, string albumName);
    }
}
