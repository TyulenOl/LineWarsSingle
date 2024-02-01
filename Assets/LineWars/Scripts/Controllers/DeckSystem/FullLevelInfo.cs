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
            var name = additionalLevelInfo.CardName.Enabled ? 
                additionalLevelInfo.CardName.Value : Name;
            var description = additionalLevelInfo.Description.Enabled ? 
                additionalLevelInfo.Description.Value : Description;
            var image = additionalLevelInfo.CardImage.Enabled ? 
                additionalLevelInfo.CardImage.Value : Image;
            var unit = additionalLevelInfo.Unit.Enabled ? 
                additionalLevelInfo.Unit.Value : Unit;
            var cost = additionalLevelInfo.Cost.Enabled ? 
                additionalLevelInfo.Cost.Value : Cost;
            
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
