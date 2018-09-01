namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;

    using Contracts;
    using Dtos;
    using Models.Enums;
    using Services.Contracts;
    using Utilities;

    public class CreateAlbumCommand : ICommand
    {
        private readonly IAlbumService albumService;
        private readonly IUserService userService;
        private readonly ITagService tagService;
        private readonly IUserSessionService userSessionService;

        public CreateAlbumCommand(IAlbumService albumService, IUserService userService, ITagService tagService, IUserSessionService userSessionService)
        {
            this.albumService = albumService;
            this.userService = userService;
            this.tagService = tagService;
            this.userSessionService = userSessionService;
        }

        // CreateAlbum <username> <albumTitle> <BgColor> <tag1> <tag2>...<tagN>
        public string Execute(string[] args)
        {
            if (args.Length < 3)
            {
                throw new ArgumentException("Command CreateAlbum not valid!");
            }

            string username = args[0];
            string albumTitle = args[1];
            string color = args[2];
            string[] tags = args.Skip(3).ToArray();

            bool userExists = this.userService.Exists(username);

            if (!userExists)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            if (!this.userSessionService.IsLoggedIn || this.userSessionService.GetUsername != username)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            bool albumExists = this.albumService.Exists(albumTitle);

            if (albumExists)
            {
                throw new ArgumentException($"Album {albumTitle} exists!");
            }

            bool isColorValid = Enum.TryParse(color, out Color backgroundColor);

            if (!isColorValid)
            {
                throw new ArgumentException($"Color {color} not found!");
            }

            for (int index = 0; index < tags.Length; index++)
            {
                tags[index] = tags[index].ValidateOrTransform();

                bool tagExists = this.tagService.Exists(tags[index]);

                if (!tagExists)
                {
                    throw new ArgumentException("Invalid tags!");
                }
            }

            int userId = this.userService.ByUsername<UserDto>(username).Id;

            this.albumService.Create(userId, albumTitle, color, tags);

            return $"Album {albumTitle} successfully created!";
        }
    }
}
