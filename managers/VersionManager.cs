using System.Collections.Generic;
using DeckTrackerCLI.Models;
using Microsoft.Data.Sqlite;

namespace DeckTrackerCLI.Managers
{
    public class VersionManager
    {
        private DatabaseInterface _db;
        private List<Version> _versions = new List<Version>();

        public VersionManager(DatabaseInterface db)
        {
            _db = db;
        }

        // accept an instance of a deck version and add it to the database, return its id
        public int CreateVersion(Version version)
        {
            version.VersionId = _db.Insert($"INSERT INTO version VALUES (null, {version.DeckId}, '{version.Name}', '{version.DeckList}', {version.AuthorId}, null)");
            _versions.Add(version);
            return version.VersionId;
        }

        // accepts a versionid and deletes version from the database if it is not in the record table
        public void DeleteVersion(int versionId)
        {
            _db.Delete($"DELETE FROM version WHERE versionid == {versionId} AND versionid NOT IN (SELECT losingversionid, winningversionid FROM record");
            _versions.Remove(_versions.Find(v => v.VersionId == versionId));
        }

        // accepts a version object and will update if it is not in the record table and create a new one if it is
        public void UpdateVersion(Version version)
        {
            bool hasRecords = false;
            string name = "";
            _db.Query($"SELECT * FROM version WHERE versionid == {version.VersionId} AND versionid NOT IN (SELECT losingversionid, winningversionid FROM record", (SqliteDataReader reader) => {
                while(reader.Read())
                {
                    hasRecords = true;
                    name = reader[2].ToString();
                }
            });
            if(hasRecords) {
                if(name == version.Name) {
                    version.Name = version.Name + "(1)";
                }
                _db.Insert($"INSERT INTO version VALUES (null, {version.DeckId}, '{version.Name}', '{version.DeckList}', {version.AuthorId}, null)");
            } else {
                _db.Update($"UPDATE version SET name = '{version.Name}', decklist = '{version.DeckList}' WHERE versionid = {version.VersionId}");
            }
        }

        // overloaded method receives a user and returns a list of all versions created by that user
        public List<Version> ListVersions(User user)
        {
            _versions.Clear();
            _db.Query($"SELECT * FROM version WHERE userid = {user.UserId}", (SqliteDataReader reader) => {
                while(reader.Read())
                {
                    _versions.Add(new Version(){
                        VersionId = reader.GetInt32(0),
                        DeckId = reader.GetInt32(1),
                        Name = reader[2].ToString(),
                        DeckList = reader[3].ToString(),
                        AuthorId = reader.GetInt32(4),
                        DateCreated = reader.GetDateTime(5)
                    });
                }
            });
            return _versions;
        }

        // overloaded method receives a deck and returns all versions of that deck
        public List<Version> ListVersions(Deck deck)
        {
            _versions.Clear();
            _db.Query($"SELECT * FROM version WHERE deckid = {deck.DeckId}", (SqliteDataReader reader) => {
                while(reader.Read())
                {
                    _versions.Add(new Version(){
                        VersionId = reader.GetInt32(0),
                        DeckId = reader.GetInt32(1),
                        Name = reader[2].ToString(),
                        DeckList = reader[3].ToString(),
                        AuthorId = reader.GetInt32(4),
                        DateCreated = reader.GetDateTime(5)
                    });
                }
            });
            return _versions;
        }
    }
}