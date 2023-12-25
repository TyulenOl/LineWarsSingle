using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(menuName = "DeckBuilding/DefaultUserInfo", order = 53)]
    public class UserInfoPreset: ScriptableObject
    {
        [SerializeField] private int defaultMoney;
        [SerializeField] private List<DeckCard> defaultCards;

        public int DefaultMoney => defaultMoney;
        public IEnumerable<DeckCard> DefaultCards => defaultCards;
    }
}