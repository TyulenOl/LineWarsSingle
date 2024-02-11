using LineWars.Model;
using UnityEngine;

namespace LineWars.Education
{
    public class UseBlessing: PlayerAction
    {
        [SerializeField] private BlessingId blessingId;

        public override bool CanSelectBlessing(BlessingId blessingId)
        {
            return this.blessingId == blessingId;
        }
    }
}