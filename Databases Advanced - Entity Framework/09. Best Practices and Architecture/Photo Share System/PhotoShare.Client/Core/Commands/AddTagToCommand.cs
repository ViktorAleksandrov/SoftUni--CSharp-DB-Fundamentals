namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Contracts;
    using Dtos;
    using Services.Contracts;
    using Utilities;

    public class AddTagToCommand : ICommand
    {
        private readonly IAlbumService albumService;
        private readonly ITagService tagService;
        private readonly IAlbumTagService albumTagService;
        private readonly IUserSessionService userSessionService;
        private readonly IAlbumRoleService albumRoleService;

        public AddTagToCommand(IAlbumService albumService, ITagService tagService, IAlbumTagService albumTagService, IUserSessionService userSessionService, IAlbumRoleService albumRoleService)
        {
            this.albumService = albumService;
            this.tagService = tagService;
            this.albumTagService = albumTagService;
            this.userSessionService = userSessionService;
            this.albumRoleService = albumRoleService;
        }

        // AddTagTo <albumName> <tag>
        public string Execute(string[] args)
        {
            if (args.Length < 2)
            {
                throw new ArgumentException("Command AddTagTo not valid!");
            }

            if (!this.userSessionService.IsLoggedIn)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            string username = this.userSessionService.GetUsername;

            string albumTitle = args[0];

            if (!this.albumRoleService.UserOwnsAlbum(username, albumTitle))
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            string tagName = args[1].ValidateOrTransform();

            bool albumExists = this.albumService.Exists(albumTitle);
            bool tagExists = this.tagService.Exists(tagName);

            if (!albumExists || !tagExists)
            {
                throw new ArgumentException("Either tag or album do not exist!");
            }

            int albumId = this.albumService.ByName<AlbumDto>(albumTitle).Id;
            int tagId = this.tagService.ByName<TagDto>(tagName).Id;

            this.albumTagService.AddTagTo(albumId, tagId);

            return $"Tag {tagName} added to {albumTitle}!";
        }
    }
}
