using System.Collections.Generic;

namespace LineWars.Model
{
    public interface IReadOnlyBasePlayer : INumbered
    {
        public int Income { get; }
        public int CurrentMoney { get; }
        public IReadOnlyNode Base { get; }
        public Nation Nation { get; }
        public PlayerRules Rules { get;}

        public IReadOnlyCollection<IReadOnlyOwned> OwnedObjects { get; }
    }
}