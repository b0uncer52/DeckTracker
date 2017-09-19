using System;

namespace DeckTrackerCLI.Models
{
    public class Record
    {
        public int RecordId { get; set; }
        public int WinnerId { get; set; }
        public int LoserId { get; set; }
        public int WinningVersionId { get; set; }
        public int LosingVersionId { get; set; }
        public DateTime DateRecorded { get; set; }
        public bool Boarded { get; set; }
        public int WinnerMull { get; set; }
        public int LoserMull { get; set; }
        public int Closeness { get; set; }
        public string Notes { get; set; }
    }
}