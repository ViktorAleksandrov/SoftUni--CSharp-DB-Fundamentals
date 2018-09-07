using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeamBuilder.Models
{
    public class Team
    {
        public Team()
        {
            this.UserTeams = new List<UserTeam>();
            this.EventTeams = new List<EventTeam>();
            this.SentInvitations = new List<Invitation>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; }

        [MaxLength(32)]
        public string Description { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Acronym { get; set; }

        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public ICollection<UserTeam> UserTeams { get; set; }

        public ICollection<EventTeam> EventTeams { get; set; }

        public ICollection<Invitation> SentInvitations { get; set; }
    }
}
