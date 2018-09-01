namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Contracts;
    using Services.Contracts;
    using Utilities;

    public class AddTagCommand : ICommand
    {
        private readonly ITagService tagService;
        private readonly IUserSessionService userSessionService;

        public AddTagCommand(ITagService tagService, IUserSessionService userSessionService)
        {
            this.tagService = tagService;
            this.userSessionService = userSessionService;
        }

        public string Execute(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentException("Command AddTag not valid!");
            }

            if (!this.userSessionService.IsLoggedIn)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            string tagName = args[0];

            bool tagExists = this.tagService.Exists(tagName);

            if (tagExists)
            {
                throw new ArgumentException($"Tag {tagName} exists!");
            }

            tagName = tagName.ValidateOrTransform();

            this.tagService.AddTag(tagName);

            return $"Tag {tagName} was added successfully!";
        }
    }
}
