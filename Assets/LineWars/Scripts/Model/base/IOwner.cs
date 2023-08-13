using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public interface IOwner 
    {
        public IReadOnlyCollection<Owned> OwnedObjects {get;}
        public void AddOwned(Owned owned);
        public void RemoveOwned(Owned owned);
    }
}

