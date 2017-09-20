using System;

namespace DeckTrackerCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseInterface db = new DatabaseInterface("deck");

            db.CheckTables();
        }

        public static void Warning(string warning)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(warning);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
