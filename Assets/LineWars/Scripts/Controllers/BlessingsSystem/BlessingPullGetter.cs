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
                return new LimitingBlessingPool(GameRoot.Instance.UserController,
                    new []{new BlessingId(BlessingType.PerunBlessing, Rarity.Common)}, 3);
            }

            return new EmptyPull();
        }
    }
}