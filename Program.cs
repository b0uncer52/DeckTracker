using System;

namespace DeckTrackerCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseInterface db = new DatabaseInterface("DECK_TRACKER_DB");
        }
    }
}
