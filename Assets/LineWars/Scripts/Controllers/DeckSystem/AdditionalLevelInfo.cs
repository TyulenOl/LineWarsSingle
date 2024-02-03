using UnityEngine;

namespace LineWars.Model
{
        [System.Serializable]
        public class AdditionalLevelInfo
        {
            [SerializeField] private Optional<string> overrideCardName;
            [SerializeField] private Optional<string> overrideDescription;
            [SerializeField] private Optional<Sprite> overrideCardImage;
            [SerializeField] private Optional<Unit> overrideUnitPrefab;
            [SerializeField] private Optional<int> overrideCostInFight;
            [SerializeField] private int costToUpgrade;

            public Optional<string> OverrideCardName => overrideCardName;
            public Optional<string> OverrideDescription => overrideDescription;
            public Optional<Sprite> OverrideCardImage => overrideCardImage;
            public Optional<Unit> OverrideUnitPrefab => overrideUnitPrefab;
            public Optional<int> OverrideCostInFight => overrideCostInFight;
            public int CostToUpgrade => costToUpgrade;
        }
}
