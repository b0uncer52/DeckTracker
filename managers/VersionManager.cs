using System.Collections.Generic;
using DeckTrackerCLI.Models;

namespace DeckTrackerCLI.Managers
{
    public class VersionManager
    {
        private DatabaseInterface _db;

        public VersionManager(DatabaseInterface db)
        {
            _db = db;
        }

        public int CreateVersion()
        {
            return 0;
        }

        public void DeleteVersion()
        {

        }

        public void UpdateVersion()
        {

        }

        public List<Version> ListUserVersions()
        {
            return new List<Version>();
        }

        public List<Version> ListDeckVersions()
        {
            return new List<Version>();
        }

        public List<Version> ListUserFormatVersions()
        {
            return new List<Version>();
        }
    }
}