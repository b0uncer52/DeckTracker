using System.Collections.Generic;
using DeckTrackerCLI.Models;

namespace DeckTrackerCLI.Managers
{
    public class RecordManager
    {
        private DatabaseInterface _db;

        public RecordManager(DatabaseInterface db)
        {
            _db = db;
        }

        public int CreateRecord()
        {
            return 0;
        }

        public List<Record> RecentRecords()
        {
            return new List<Record>();
        }

        public List<Record> UserRecords()
        {
            return new List<Record>();
        }

        public List<Record> DeckRecords()
        {
            return new List<Record>();
        }

        public List<Record> VersionRecords()
        {
            return new List<Record>();
        }

        public List<Record> FormatRecords()
        {
            return new List<Record>();
        }

        public List<Record> TeamRecords()
        {
            return new List<Record>();
        }
    }
}