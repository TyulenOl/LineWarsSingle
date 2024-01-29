using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BlessingsGroupDrawer : MonoBehaviour
    {
        [SerializeField] private BlessingDraggableSet blessingDraggableSetPrefab;
        [SerializeField] private LayoutGroup layoutToGenerateBlessings;
        [SerializeField] private TMP_Text groupName;

        private Dictionary<Rarity, BlessingDraggableSet> rarityToSet;

        private BlessingType blessingType;
        public BlessingType BlessingType => blessingType;
        public IReadOnlyDictionary<Rarity, BlessingDraggableSet> RarityToSet => rarityToSet;

        public void Initialize(BlessingType blessingType)
        {
            this.blessingType = blessingType;
            rarityToSet = new Dictionary<Rarity, BlessingDraggableSet>();
            groupName.text = DrawHelper.GetBlessingTypeName(blessingType);
        }

        public void AddRarity(Rarity rarity)
        {
            if (ContainsRarity(rarity))
                return;
            var instance = Instantiate(blessingDraggableSetPrefab, layoutToGenerateBlessings.transform);
            instance.Redraw(new BlessingId(BlessingType, rarity));
            rarityToSet[rarity] = instance;
        }

        public void RemoveRarity(Rarity rarity)
        {
            if (ContainsRarity(rarity))
                return;
            Destroy(rarityToSet[rarity].gameObject);
        }

        public bool ContainsRarity(Rarity rarity)
        {
            return rarityToSet.ContainsKey(rarity);
        }
    }
}
