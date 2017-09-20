using System.Collections.Generic;
using DeckTrackerCLI.Models;

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

        // accept a deckId and remove it from the database
        public void DeleteDeck(int deckId) 
        {
            _db.Delete($"DELETE FROM deck WHERE deckid == {deckId}");
            _decks.Remove(_decks.Find(d => d.DeckId == deckId));
        }

        public List<Deck> ListDecks() // should accept format
        {
            return new List<Deck>();
        }
    }
}