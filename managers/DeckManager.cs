using System.Collections.Generic;
using DeckTrackerCLI.Models;
using Microsoft.Data.Sqlite;

namespace DeckTrackerCLI.Managers
{
    public class DeckManager
    {
        private DatabaseInterface _db;
        private List<Deck> _decks;

        public DeckManager(DatabaseInterface db)
        {
            _db = db;
        }

        // accpet an instance of a deck and add it to the database, returning its id
        public int CreateDeck(Deck deck)
        {
            deck.DeckId = _db.Insert($"INSERT INTO deck VALUES (null, {deck.Name}, {deck.FormatId})");
            _decks.Add(deck);
            return deck.DeckId;
        }

        // accept formatid and return all decks in that format
        public List<Deck> ListDecks(int formatId)
        {
            _decks.Clear();
            _db.Query($"SELECT * FROM deck WHERE formatid == {formatId}", (SqliteDataReader reader) => {
                while(reader.Read())
                {
                    _decks.Add(new Deck(){
                        DeckId = reader.GetInt32(0),
                        Name = reader[1].ToString(),
                        FormatId = reader.GetInt32(2)
                    });
                }
            });
            return _decks;
        }
    }
}