using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    /// <summary>
    /// класс, содержащий всю логику, которая объединяет ИИ и игрока
    /// </summary>
    public abstract class BasePlayer: MonoBehaviour
    {
        public SpawnInfo SpawnInfo { get; set; }
        public int Money { get; set; }

        private HashSet<Owned> myOwned = new();
        public IReadOnlyCollection<Owned> OwnedObjects => myOwned;
        public bool IsMyOwn(Owned owned) => myOwned.Contains(owned);

        public void AddOwned(Owned owned)
        {
            if (owned != null)
                myOwned.Add(owned);
        }

        public void RemoveOwned(Owned owned)
        {
            myOwned.Remove(owned);
        }
    }
}

