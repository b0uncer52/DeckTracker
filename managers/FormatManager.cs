using System.Collections.Generic;
using DeckTrackerCLI.Models;
using Microsoft.Data.Sqlite;

namespace DeckTrackerCLI.Managers
{
    public class FormatManager
    {
        private DatabaseInterface _db;
        private List<Format> _formats = new List<Format>();
        
        public FormatManager(DatabaseInterface db)
        {
            _db = db;
        }

        // accept a new format and create an instance in the database, returning its id
        public int CreateFormat(Format format)
        {
            format.FormatId = _db.Insert($"INSERT INTO format VALUES (null, {format.Name})");
            _formats.Add(format);
            return format.FormatId;
        }

        // return a list of all formats in the database
        public List<Format> ListFormats()
        {
            _db.Query($"SELECT * FROM format", (SqliteDataReader reader) =>{
                while(reader.Read())
                {
                    _formats.Add(new Format(){
                        FormatId = reader.GetInt32(0),
                        Name =  reader[1].ToString()
                    });
                }
            });
            return _formats;
        }
    }
}