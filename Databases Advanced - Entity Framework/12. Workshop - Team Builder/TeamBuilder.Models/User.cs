using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TeamBuilder.Models.Enums;

namespace TeamBuilder.Models
{
    public class User
    {
        public User()
        {
            this.CreatedEvents = new List<Event>();
            this.UserTeams = new List<UserTeam>();
            this.CreatedTeams = new List<Team>();
            this.ReceivedInvitations = new List<Invitation>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 6)]
        public string Password { get; set; }

        [MaxLength(25)]
        public string FirstName { get; set; }

        [MaxLength(25)]
        public string LastName { get; set; }

        public int Age { get; set; }

        public Gender Gender { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Event> CreatedEvents { get; set; }

        public ICollection<UserTeam> UserTeams { get; set; }

        public ICollection<Team> CreatedTeams { get; set; }

        public ICollection<Invitation> ReceivedInvitations { get; set; }
    }
}
