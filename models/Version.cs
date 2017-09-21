using System;

namespace DeckTrackerCLI.Models
{
    public class Version
    {
        public int VersionId { get; set; }
        public int DeckId { get; set; }
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string DeckList { get; set; }
        public DateTime DateCreated { get; set; }
    }
}