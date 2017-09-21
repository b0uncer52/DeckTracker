using System;
using System.Collections.Generic;
using DeckTrackerCLI.Models;
using Microsoft.Data.Sqlite;

namespace DeckTrackerCLI.Managers
{
    public class RecordManager
    {
        private DatabaseInterface _db;
        private List<Record> _records = new List<Record>();

        public RecordManager(DatabaseInterface db)
        {
            _db = db;
        }

        // receive record object and add it to the database, returning its id
        public int CreateRecord(Record record)
        {
            record.RecordId = _db.Insert($"INSERT INTO record VALUES (null, {record.WinnerId}, {record.LoserId}, {record.WinningVersionId}, {record.LosingVersionId}, null, {record.Boarded}, {record.WinnerMull}, {record.LoserMull}, {record.Closeness}, {record.Notes})");
            _records.Add(record);
            return record.RecordId;
        }

        // return all records made in the last 10 days
        public List<Record> RecentRecords()
        {
            _records.Clear();
            _db.Query($"SELECT * FROM record WHERE daterecorded > {DateTime.Now.Subtract(new DateTime(0,0,10))}", (SqliteDataReader reader) => {
                while(reader.Read()) {
                    _records.Add(new Record(){
                        RecordId = reader.GetInt32(0),
                        WinnerId = reader.GetInt32(1),
                        LoserId = reader.GetInt32(2),
                        WinningVersionId = reader.GetInt32(3),
                        LosingVersionId = reader.GetInt32(4),
                        DateRecorded = reader.GetDateTime(5),
                        Boarded = reader.GetInt32(6).Equals(1),
                        WinnerMull = reader.GetInt32(7),
                        LoserMull = reader.GetInt32(8),
                        Closeness = reader.GetInt32(9),
                        Notes = reader[10].ToString()
                    });
                }
            });
            return _records;
        }

        // return all records with given userId as winner or loser
        public List<Record> UserRecords(int userId)
        {
            _records.Clear();
            _db.Query($"SELECT * FROM record WHERE winnerid = {userId} OR loserid = {userId}", (SqliteDataReader reader) => {
                while(reader.Read()){
                    _records.Add(new Record(){
                        RecordId = reader.GetInt32(0),
                        WinnerId = reader.GetInt32(1),
                        LoserId = reader.GetInt32(2),
                        WinningVersionId = reader.GetInt32(3),
                        LosingVersionId = reader.GetInt32(4),
                        DateRecorded = reader.GetDateTime(5),
                        Boarded = reader.GetInt32(6).Equals(1),
                        WinnerMull = reader.GetInt32(7),
                        LoserMull = reader.GetInt32(8),
                        Closeness = reader.GetInt32(9),
                        Notes = reader[10].ToString()
                    });
                }
            });
            return _records;
        }

        // returns all records for a given deck, cross-refencing the versions table
        public List<Record> DeckRecords(int deckId)
        {
            _records.Clear();
            _db.Query($"SELECT * FROM record WHERE winningversionid  OR losingversionid IS IN (SELECT versionid FROM version WHERE deckid = {deckId})", (SqliteDataReader reader) => {
                while(reader.Read()){
                    _records.Add(new Record(){
                        RecordId = reader.GetInt32(0),
                        WinnerId = reader.GetInt32(1),
                        LoserId = reader.GetInt32(2),
                        WinningVersionId = reader.GetInt32(3),
                        LosingVersionId = reader.GetInt32(4),
                        DateRecorded = reader.GetDateTime(5),
                        Boarded = reader.GetInt32(6).Equals(1),
                        WinnerMull = reader.GetInt32(7),
                        LoserMull = reader.GetInt32(8),
                        Closeness = reader.GetInt32(9),
                        Notes = reader[10].ToString()
                    });
                }
            });
            return _records;
        }

        // return all records for a given versionId
        public List<Record> VersionRecords(int versionId)
        {
            _records.Clear();
            _db.Query($"SELECT * FROM record WHERE winningversionid = {versionId} OR losingversionid = {versionId}", (SqliteDataReader reader) => {
                while(reader.Read()){
                    _records.Add(new Record(){
                        RecordId = reader.GetInt32(0),
                        WinnerId = reader.GetInt32(1),
                        LoserId = reader.GetInt32(2),
                        WinningVersionId = reader.GetInt32(3),
                        LosingVersionId = reader.GetInt32(4),
                        DateRecorded = reader.GetDateTime(5),
                        Boarded = reader.GetInt32(6).Equals(1),
                        WinnerMull = reader.GetInt32(7),
                        LoserMull = reader.GetInt32(8),
                        Closeness = reader.GetInt32(9),
                        Notes = reader[10].ToString()
                    });
                }
            });
            return _records;
        }

        // return all records with versions of decks in the given format
        public List<Record> FormatRecords(int formatId)
        {
            _records.Clear();
            _db.Query($"SELECT * FROM record WHERE winningversionid OR loserid IS IN (SELECT versionid FROM version WHERE deckid IS IN (SELECT deckid FROM deck WHERE formatid = {formatId}))", (SqliteDataReader reader) => {
                while(reader.Read()){
                    _records.Add(new Record(){
                        RecordId = reader.GetInt32(0),
                        WinnerId = reader.GetInt32(1),
                        LoserId = reader.GetInt32(2),
                        WinningVersionId = reader.GetInt32(3),
                        LosingVersionId = reader.GetInt32(4),
                        DateRecorded = reader.GetDateTime(5),
                        Boarded = reader.GetInt32(6).Equals(1),
                        WinnerMull = reader.GetInt32(7),
                        LoserMull = reader.GetInt32(8),
                        Closeness = reader.GetInt32(9),
                        Notes = reader[10].ToString()
                    });
                }
            });
            return _records;
        }

        // return all records with both players in a given team
        public List<Record> TeamRecords(int teamId)
        {
            _records.Clear();
            _db.Query($"SELECT * FROM record WHERE winnerid AND loserid IS IN (SELECT userid FROM teammember WHERE teamid = {teamId})", (SqliteDataReader reader) => {
                while(reader.Read()){
                    _records.Add(new Record(){
                        RecordId = reader.GetInt32(0),
                        WinnerId = reader.GetInt32(1),
                        LoserId = reader.GetInt32(2),
                        WinningVersionId = reader.GetInt32(3),
                        LosingVersionId = reader.GetInt32(4),
                        DateRecorded = reader.GetDateTime(5),
                        Boarded = reader.GetInt32(6).Equals(1),
                        WinnerMull = reader.GetInt32(7),
                        LoserMull = reader.GetInt32(8),
                        Closeness = reader.GetInt32(9),
                        Notes = reader[10].ToString()
                    });
                }
            });
            return _records;
        }
    }
}