using UnityEngine;

namespace LineWars.Model
{
    public class FullLevelInfo
    {
        public string Name { get; } 
        public string Description { get; }
        public Sprite Image{ get; }
        public Unit Unit { get; }
        public int Cost {get;}
        public int CostToUpgrade { get; }

        public FullLevelInfo(
            string name, 
            string description, 
            Sprite image, 
            Unit unit, 
            int cost,
            int costToUpgrade)
        {
            Name = name;
            Description = description;
            Image = image;
            Unit = unit;
            Cost = cost;
            CostToUpgrade = costToUpgrade;
        }

        public FullLevelInfo WithAdditionalInfo(AdditionalLevelInfo additionalLevelInfo)
        {
            var name = additionalLevelInfo.OverrideCardName.Enabled ? 
                additionalLevelInfo.OverrideCardName.Value : Name;
            var description = additionalLevelInfo.OverrideDescription.Enabled ? 
                additionalLevelInfo.OverrideDescription.Value : Description;
            var image = additionalLevelInfo.OverrideCardImage.Enabled ? 
                additionalLevelInfo.OverrideCardImage.Value : Image;
            var unit = additionalLevelInfo.OverrideUnitPrefab.Enabled ? 
                additionalLevelInfo.OverrideUnitPrefab.Value : Unit;
            var cost = additionalLevelInfo.OverrideCostInFight.Enabled ? 
                additionalLevelInfo.OverrideCostInFight.Value : Cost;
            
            return new FullLevelInfo(
                name,
                description,
                image,
                unit,
                cost,
                additionalLevelInfo.CostToUpgrade);

        }
    }
}
