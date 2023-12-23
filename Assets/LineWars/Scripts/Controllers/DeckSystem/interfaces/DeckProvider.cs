using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public abstract class DeckProvider: ScriptableObject, IProvider<Deck>
    {
        public abstract void Save(Deck value, int id);
        public abstract Deck Load(int id);
        public abstract IEnumerable<Deck> LoadAll();
    }
}