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
        public UnitCost CostProgression { get; }
        public int CostToUpgrade { get; }

        public FullLevelInfo(
            string name, 
            string description, 
            Sprite image, 
            Unit unit, 
            int cost,
            UnitCost costProgression,
            int costToUpgrade)
        {
            Name = name;
            Description = description;
            Image = image;
            Unit = unit;
            Cost = cost;
            CostProgression = costProgression;
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
            var costProgression = additionalLevelInfo.OverrideCostProgression.Enabled ?
                additionalLevelInfo.OverrideCostProgression.Value : CostProgression;
            
            return new FullLevelInfo(
                name,
                description,
                image,
                unit,
                cost,
                costProgression,
                additionalLevelInfo.CostToUpgrade);

        }
    }
}
