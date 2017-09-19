using System;
using Microsoft.Data.Sqlite;

namespace DeckTrackerCLI
{
    public class DatabaseInterface
    {
        private SqliteConnection _connection;

        public DatabaseInterface(string database)
        {
            _connection = new SqliteConnection($"Data Source={Environment.GetEnvironmentVariable(database)}");
        }

        public void Query(string command, Action<SqliteDataReader> handler)
        {
            using(_connection)
            {
                _connection.Open();
                SqliteCommand dbcmd = _connection.CreateCommand();
                dbcmd.CommandText = command;
                using(SqliteDataReader dataReader = dbcmd.ExecuteReader()){
                    handler(dataReader);
                }
                dbcmd.Dispose();
                _connection.Close();
            }
        }

        public void Delete(string command)
        {
            using (_connection)
            {
                _connection.Open();
                SqliteCommand dbcmd = _connection.CreateCommand();
                dbcmd.CommandText = command;
                
                dbcmd.ExecuteNonQuery();
                
                dbcmd.Dispose();
                _connection.Close();
            }
        }

        public int Insert(string command)
        {
            int insertedItemId = 0;

            using (_connection)
            {
                _connection.Open ();
                SqliteCommand dbcmd = _connection.CreateCommand ();
                dbcmd.CommandText = command;
                
                dbcmd.ExecuteNonQuery ();

                this.Query("select last_insert_rowid()",
                    (SqliteDataReader reader) => {
                        while (reader.Read ())
                        {
                            insertedItemId = reader.GetInt32(0);
                        }
                    }
                );

                dbcmd.Dispose ();
                _connection.Close ();
            }

            return insertedItemId;
        }

        public void Update(string command)
        {
            using (_connection)
            {
                _connection.Open ();
                SqliteCommand dbcmd = _connection.CreateCommand ();
                dbcmd.CommandText = command;
                
                dbcmd.ExecuteNonQuery ();

                dbcmd.Dispose ();
                _connection.Close ();
            }
        }

        public void CheckTables ()
        {
            using(_connection)
            {
                _connection.Open();
                SqliteCommand dbcmd = _connection.CreateCommand();

                dbcmd.CommandText = $"SELECT UserId FROM User";
                try {
                    using(SqliteDataReader reader = dbcmd.ExecuteReader()){}
                    dbcmd.Dispose();
                } catch(Microsoft.Data.Sqlite.SqliteException ex){
                    if(ex.Message.Contains("no such table")) {
                        dbcmd.CommandText = $@"CREATE TABLE User (`UserID` integer NOT NULL PRIMARY KEY AUTOINCREMENT, `Name` varchar(20) NOT NULL, `Password` varchar(20) NOT NULL, `DateCreated` DATE DEFAULT (datetime('now', 'localtime')), `Email` varchar(50) NOT NULL)";
                        dbcmd.ExecuteNonQuery();          
                        dbcmd.Dispose();
                    }
                }

                dbcmd.CommandText = $"SELECT FormatId FROM Format";

                try {
                    using (SqliteDataReader reader = dbcmd.ExecuteReader()){}
                    dbcmd.Dispose();
                } catch (Microsoft.Data.Sqlite.SqliteException ex) {
                    if(ex.Message.Contains("no such table"))
                    {
                        dbcmd.CommandText = $@"CREATE TABLE Format (`FormatId` integer NOT NULL PRIMARY KEY AUTOINCREMENT, `Name` varchar(20) NOT NULL)";
                        dbcmd.ExecuteNonQuery();
                        dbcmd.Dispose();
                    }
                }

                dbcmd.CommandText = $"SELECT DeckId FROM Deck";
                try {
                    using(SqliteDataReader reader = dbcmd.ExecuteReader()){}
                    dbcmd.Dispose();
                } catch(Microsoft.Data.Sqlite.SqliteException ex){
                    if(ex.Message.Contains("no such table")) {
                        dbcmd.CommandText = $@"CREATE TABLE Deck (`DeckId` integer NOT NULL PRIMARY KEY AUTOINCREMENT, `Name` varchar(20) NOT NULL, `FormatId` integer NOT NULL, FOREIGN KEY(`FormatId`) REFERENCES `Format` (`FormatId`))";
                        dbcmd.ExecuteNonQuery();
                        dbcmd.Dispose();
                    }
                }

                dbcmd.CommandText = $"SELECT TeamId FROM Team";
                try {
                    using (SqliteDataReader reader = dbcmd.ExecuteReader()){}
                    dbcmd.Dispose();
                } catch (Microsoft.Data.Sqlite.SqliteException ex) {
                    if(ex.Message.Contains("no such table")) 
                    {
                        dbcmd.CommandText = $@"CREATE TABLE Team (`TeamId` integer NOT NULL PRIMARY KEY AUTOINCREMENT, `Name` varchar(50) NOT NULL, `CreatorId` integer NOT NULL, `DateCreated` DATE DEFAULT (datetime('now', 'localtime')), FOREIGN KEY (`CreatorId`) REFERENCES `User` (`UserId`))";
                        dbcmd.ExecuteNonQuery();
                        dbcmd.Dispose();
                    }
                }

                dbcmd.CommandText = $"SELECT TeamMemberId FROM TeamMember";
                try {
                    using (SqliteDataReader reader = dbcmd.ExecuteReader()) {}
                    dbcmd.Dispose();
                } catch (Microsoft.Data.Sqlite.SqliteException ex) {
                    if(ex.Message.Contains("no such table")) {
                        dbcmd.CommandText = $@"CREATE TABLE TeamMember (`TeamMemberId` integer NOT NULL PRIMARY KEY AUTOINCREMENT, `UserId` integer NOT NULL, `TeamId` integer NOT NULL, `DateJoined` DATE, `Accepted` integer NOT NULL, FOREIGN KEY (`UserId`) REFERENCES `User` (`UserId`), FOREIGN KEY (`TeamId`) REFERENCES `Team` (`TeamId`))";
                        dbcmd.ExecuteNonQuery();
                        dbcmd.Dispose();
                    }
                }

                dbcmd.CommandText = $"SELECT KeyId FROM KeyToWin";
                try {
                    using (SqliteDataReader reader = dbcmd.ExecuteReader()) {}
                    dbcmd.Dispose();
                } catch (Microsoft.Data.Sqlite.SqliteException ex) {
                    if(ex.Message.Contains("no such table")) {
                        dbcmd.CommandText = $@"CREATE TABLE KeyToWin (`KeyId` integer NOT NULL PRIMARY KEY AUTOINCREMENT, `Name` varchar(50) NOT NULL)";
                        dbcmd.ExecuteNonQuery();
                        dbcmd.Dispose();
                    }
                }

                dbcmd.CommandText = $"SELECT VersionId FROM Version";
                try {
                    using (SqliteDataReader reader = dbcmd.ExecuteReader()) {}
                    dbcmd.Dispose();
                } catch (Microsoft.Data.Sqlite.SqliteException ex) {
                    if(ex.Message.Contains("no such table")) {
                        dbcmd.CommandText = $@"CREATE TABLE Version (`VersionId` integer NOT NULL PRIMARY KEY AUTOINCREMENT, `DeckId` integer NOT NULL, `Name` varchar(20) NOT NULL, `DeckList` varchar(1000) NOT NULL, `AuthorId` integer NOT NULL, `DateCreated` DATE DEFAULT (datetime('now', 'localtime')), FOREIGN KEY (`DeckId`) REFERENCES `Deck` (`DeckId`), FOREIGN KEY (`AuthorId`) REFERENCES `User` (`UserId`))";
                        dbcmd.ExecuteNonQuery();
                        dbcmd.Dispose();
                    }
                }

                dbcmd.CommandText = $"SELECT RecordId FROM Record";
                try {
                    using (SqliteDataReader reader = dbcmd.ExecuteReader()) {}
                    dbcmd.Dispose();
                } catch (Microsoft.Data.Sqlite.SqliteException ex) {
                    if(ex.Message.Contains("no such table")) {
                        dbcmd.CommandText = $@"CREATE TABLE Record (`RecordId` integer NOT NULL PRIMARY KEY AUTOINCREMENT, `WinnerId` integer NOT NULL, `LoserId` integer NOT NULL, `WinningVersionId` integer NOT NULL, `LosingVersionId` integer NOT NULL, `DateRecorded` DATE DEFAULT (datetime('now', 'localtime')), `Boarded` integer NOT NUll, `WinnerMull` integer NOT NULL, `LoserMull` integer NOT NULL, `Closeness` integer NOT NULL, `Notes` varchar(200), FOREIGN KEY (`WinnerId`) REFERENCES `User` (`UserId`), FOREIGN KEY (`LoserId`) REFERENCES `User` (`UserId`), FOREIGN KEY (`WinningVersionId`) REFERENCES `Version` (`VersionId`), FOREIGN KEY (`LosingVersionId`) REFERENCES `Version` (`VersionId`))";
                        dbcmd.ExecuteNonQuery();
                        dbcmd.Dispose();
                    }
                }

                dbcmd.CommandText = $"SELECT RecordKeyId FROM RecordKey";
                try {
                    using(SqliteDataReader reader = dbcmd.ExecuteReader()) {}
                    dbcmd.Dispose();
                } catch (Microsoft.Data.Sqlite.SqliteException ex) {
                    if(ex.Message.Contains("no such table")) {
                        dbcmd.CommandText = $@"CREATE TABLE RecordKey (`RecordKeyId` integer NOT NULL PRIMARY KEY AUTOINCREMENT, `RecordId` integer NOT NULL, `KeyId` integer NOT NULL, FOREIGN KEY (`RecordId`) REFERENCES `Record` (`RecordId`), FOREIGN KEY (`KeyId`) REFERENCES `KeyToWin` (`KeyId`))";
                        dbcmd.ExecuteNonQuery();
                        dbcmd.Dispose();
                    }
                }
            }
        }
    }
}