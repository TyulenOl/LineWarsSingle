
using System.Collections.Generic;

namespace LineWars.Model
{
    public interface IDeck<out TDeckCard>
        where TDeckCard : IDeckCard
    {
        public int Id { get; }
        public string Name { get; }
        public IReadOnlyList<TDeckCard> Cards { get; }
    }
}
