using System;

namespace DeckTrackerCLI.Models
{
    public class TeamMember
    {
        public int TeamMemberId { get; set; }
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public DateTime DateJoined { get; set; }
        public bool Accepted { get; set; }
    }
}