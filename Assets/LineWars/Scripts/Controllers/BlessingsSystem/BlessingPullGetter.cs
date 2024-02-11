using System;
using System.Linq;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class BlessingPullGetter: MonoBehaviour, IGetter<IBlessingsPull>
    {
        [SerializeField] protected bool isGlobal;
        
        public virtual bool CanGet()
        {
            return GameRoot.Instance != null;
        }

        public virtual IBlessingsPull Get()
        {
            if (GameRoot.Instance != null)
            {
                
                return isGlobal
                    ?GameRoot.Instance.UserController 
                    :GameRoot.Instance.BlessingsController.BlissingPullForGame;
            }

            return new EmptyPull();
        }
    }
}