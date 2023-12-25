using LineWars.Model;

namespace LineWars
{
    public class CardSlotModel
    {
        private UnitSize unitSize;

        public UnitSize UnitSize => unitSize;

        public CardSlotModel(UnitSize unitSize)
        {
            this.unitSize = unitSize;
        }
    }
}