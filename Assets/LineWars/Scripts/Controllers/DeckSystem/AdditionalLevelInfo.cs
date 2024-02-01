using UnityEngine;

namespace LineWars.Model
{
        [System.Serializable]
        public class AdditionalLevelInfo
        {
            [SerializeField] private Optional<string> cardName;
            [SerializeField] private Optional<string> description;
            [SerializeField] private Optional<Sprite> cardImage;
            [SerializeField] private Optional<Unit> unit;
            [SerializeField] private Optional<Sprite> cardActiveBagLine;
            [SerializeField] private Optional<Sprite> cardInactiveBagLine;
            [SerializeField] private Optional<int> cost;
            [SerializeField] private int costToUpgrade;

            public Optional<string> CardName => cardName;
            public Optional<string> Description => description;
            public Optional<Sprite> CardImage => cardImage;
            public Optional<Unit> Unit => unit;
            public Optional<Sprite> CardActiveBagLine => cardActiveBagLine;
            public Optional<Sprite> CardInactiveBagLine => cardInactiveBagLine;
            public Optional<int> Cost => cost;
            public int CostToUpgrade => costToUpgrade;
        }
}
