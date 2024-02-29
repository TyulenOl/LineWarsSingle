
using System.Collections.Generic;

namespace LineWars.Model
{
    public interface IDeck<out TDeckCard>
        where TDeckCard : IDeckCard
    {
        public int Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<TDeckCard> Cards { get; }
    }
}
