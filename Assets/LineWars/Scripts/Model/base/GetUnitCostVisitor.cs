using System.Linq;

namespace LineWars.Model
{
    public class GetUnitCostVisitor: IBasePlayerVisitor<int>
    {
        private UnitType type;

        public GetUnitCostVisitor(UnitType type)
        {
            this.type = type;
        }
        public int Visit(BasePlayer basePlayer)
        {
            var unitCount = basePlayer.MyUnits.Count(unit => unit.Type == type);
            return 0;
        }

        public int Visit(BasePlayerProjection projection)
        {
            throw new System.NotImplementedException();
        }
    }
}