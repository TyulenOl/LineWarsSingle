using System.Collections.Generic;

namespace LineWars.Model
{
    public interface IDeckBuilder<TDeck, TDeckCard>
        where TDeck: IDeck<TDeckCard>
        where TDeckCard: IDeckCard
    {
        public int DeckId { get; }
        public string DeckName { get; }
        public IEnumerable<TDeckCard> CurrentCards { get; }
        
        public void SetId(int id);
        public void SetName(string name);
        
        public bool AddCard(TDeckCard card);
        public bool RemoveCard(TDeckCard card);
        public bool CanBuild();
        public TDeck Build();
    }
}