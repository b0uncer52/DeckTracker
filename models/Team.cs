using System;

namespace DeckTrackerCLI.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public int CreatorId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}