using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class DefaultOpenedCards: ScriptableObject
    {
        [SerializeField] private List<DeckCard> defaultCards;

        public IEnumerable<DeckCard> DefaultCards => defaultCards;
    }
}