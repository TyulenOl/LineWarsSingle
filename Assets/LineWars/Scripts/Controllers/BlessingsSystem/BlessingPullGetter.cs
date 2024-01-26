using System;
using System.Linq;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class BlessingPullGetter: MonoBehaviour, IGetter<IBlessingsPull>
    {
        public virtual bool CanGet()
        {
            return GameRoot.Instance != null;
        }

        public virtual IBlessingsPull Get()
        {
            if (GameRoot.Instance != null)
            {
                //временно
                return GameRoot.Instance.UserController;
                // return new LimitingBlessingPool(GameRoot.Instance.UserController,
                //     new []
                //     {
                //         new BlessingId(BlessingType.PerunBlessing, Rarity.Common),
                //         new BlessingId(BlessingType.StribogBlessing, Rarity.Rare),
                //         new BlessingId(BlessingType.PowerBlessing, Rarity.Rare)
                //     }, 
                //     3);
            }

            return new EmptyPull();
        }
    }
}