namespace LineWars.Model
{
    public class Fortification: Building, IFortification
    {
        public int MaxHp { get; set; }
        public int CurrentHp { get; set; }
        
        public override BuildingType BuildingType => BuildingType.Fortification;
    }
}