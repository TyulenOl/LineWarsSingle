using LineWars.Model;
using UnityEngine;

namespace LineWars.Education
{
    public class UseBlessingPlayerAction: PlayerAction
    {
        [SerializeField] private BlessingId blessingId;

        public override bool CanSelectBlessing(BlessingId blessingId)
        {
            return this.blessingId == blessingId;
        }
    }
}