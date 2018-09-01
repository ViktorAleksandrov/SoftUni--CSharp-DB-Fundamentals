namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Contracts;
    using Dtos;
    using Services.Contracts;

    public class UploadPictureCommand : ICommand
    {
        private readonly IPictureService pictureService;
        private readonly IAlbumService albumService;
        private readonly IUserSessionService userSessionService;
        private readonly IAlbumRoleService albumRoleService;

        public UploadPictureCommand(IPictureService pictureService, IAlbumService albumService, IUserSessionService userSessionService, IAlbumRoleService albumRoleService)
        {
            this.pictureService = pictureService;
            this.albumService = albumService;
            this.userSessionService = userSessionService;
            this.albumRoleService = albumRoleService;
        }

        // UploadPicture <albumName> <pictureTitle> <pictureFilePath>
        public string Execute(string[] args)
        {
            if (args.Length < 3)
            {
                throw new ArgumentException("Command UploadPicture not valid!");
            }

            if (!this.userSessionService.IsLoggedIn)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            string albumName = args[0];
            string pictureTitle = args[1];
            string path = args[2];

            bool albumExists = this.albumService.Exists(albumName);

            if (!albumExists)
            {
                throw new ArgumentException($"Album {albumName} not found!");
            }

            string username = this.userSessionService.GetUsername;
            int albumId = this.albumService.ByName<AlbumDto>(albumName).Id;

            if (!this.albumRoleService.UserOwnsAlbum(username, albumName))
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            this.pictureService.Create(albumId, pictureTitle, path);

            return $"Picture {pictureTitle} added to {albumName}!";
        }
    }
}
