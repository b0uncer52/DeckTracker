using System.Collections.Generic;
using DeckTrackerCLI.Models;
using Microsoft.Data.Sqlite;

namespace DeckTrackerCLI.Managers
{
    public class KeyToWinManager
    {
        private DatabaseInterface _db;
        private List<KeyToWin> _keys;

        public KeyToWinManager(DatabaseInterface db)
        {
            _db = db;
        }

        // create a new key, accepts the name of that key and returns the assigned id
        public int CreateKey(KeyToWin key)
        {
            key.KeyId = _db.Insert($"INSERT INTO keytowin VALUES (null, '{key.Name}')");
            _keys.Add(key);
            return key.KeyId;
        }

        // accept a key's id and remove it and all join instances from the database
        public void DeleteKey(int keyId)
        {

            _db.Delete($"DELETE FROM recordkey WHERE keyid == {keyId}");
            _db.Delete($"DELETE FROM keytowin WHERE keyid == {keyId}");
            _keys.Remove(_keys.Find(k => k.KeyId == keyId));
        }

        // return a list of all keys in the database
        public List<KeyToWin> ListKeys()
        {
            _keys.Clear();
            _db.Query($"SELECT keyid, name FROM keytowin", (SqliteDataReader reader) => {
                    while (reader.Read ())
                    {
                        _keys.Add(new KeyToWin(){
                            KeyId = reader.GetInt32(0),
                            Name = reader[1].ToString()
                        });
                    }
                });
            return _keys;
        }
    }
}