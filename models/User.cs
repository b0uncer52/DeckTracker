using System;

namespace DeckTrackerCLI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
        public string Email { get; set; }
    }
}