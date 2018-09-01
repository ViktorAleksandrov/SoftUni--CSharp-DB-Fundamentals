namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Contracts;
    using Dtos;
    using Models.Enums;
    using Services.Contracts;

    public class ShareAlbumCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly IAlbumService albumService;
        private readonly IAlbumRoleService albumRoleService;
        private readonly IUserSessionService userSessionService;

        public ShareAlbumCommand(IUserService userService, IAlbumService albumService, IAlbumRoleService albumRoleService, IUserSessionService userSessionService)
        {
            this.userService = userService;
            this.albumService = albumService;
            this.albumRoleService = albumRoleService;
            this.userSessionService = userSessionService;
        }
        // ShareAlbum <albumId> <username> <permission>
        // For example:
        // ShareAlbum 4 dragon321 Owner
        // ShareAlbum 4 dragon11 Viewer
        public string Execute(string[] args)
        {
            if (args.Length < 3)
            {
                throw new ArgumentException("Command ShareAlbum not valid!");
            }

            int albumId = int.Parse(args[0]);
            string username = args[1];
            string permission = args[2];

            bool userExists = this.userService.Exists(username);

            if (!userExists)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            if (username != this.userSessionService.GetUsername && permission == "Owner")
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            bool albumExists = this.albumService.Exists(albumId);

            if (!albumExists)
            {
                throw new ArgumentException($"Album {albumId} not found!");
            }

            bool isPermissionValid = Enum.TryParse(permission, out Role role);

            if (!isPermissionValid)
            {
                throw new ArgumentException("Permission must be either \"Owner\" or \"Viewer\"!");
            }

            int userId = this.userService.ByUsername<UserDto>(username).Id;
            string albumTitle = this.albumService.ById<AlbumDto>(albumId).Name;

            this.albumRoleService.PublishAlbumRole(albumId, userId, permission);

            return $"Username {username} added to album {albumTitle} ({permission})";
        }
    }
}
