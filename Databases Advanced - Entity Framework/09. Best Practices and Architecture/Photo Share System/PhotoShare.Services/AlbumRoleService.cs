namespace PhotoShare.Services
{
    using System;
    using System.Linq;

    using Contracts;
    using Data;
    using Models;
    using Models.Enums;

    public class AlbumRoleService : IAlbumRoleService
    {
        private readonly PhotoShareContext context;

        public AlbumRoleService(PhotoShareContext context)
        {
            this.context = context;
        }

        public AlbumRole PublishAlbumRole(int albumId, int userId, string role)
        {
            Role roleAsEnum = Enum.Parse<Role>(role);

            var albumRole = new AlbumRole()
            {
                AlbumId = albumId,
                UserId = userId,
                Role = roleAsEnum
            };

            this.context.AlbumRoles.Add(albumRole);

            this.context.SaveChanges();

            return albumRole;
        }

        public bool UserOwnsAlbum(string username, string albumName)
        {
            return this.context.AlbumRoles
                        .Any(a => a.User.Username == username && a.Album.Name == albumName && a.Role == Role.Owner);
        }
    }
}
