﻿using System;

namespace LineWars.Model
{
    public abstract class OwnedProjection : IOwned
    {
        private BasePlayerProjection owner;
        public int OwnerId => owner?.Id ?? -1;
        public BasePlayerProjection Owner 
        {
            get => owner;
            set
            {
                var oldOwner = owner;
                owner = value;
                if (oldOwner != owner)
                {
                    OwnerChanged?.Invoke(this, oldOwner, value);
                    if(oldOwner != null)
                        oldOwner.RemoveOwned(this);
                }    
                    
            }
        }

        public event Action<OwnedProjection, BasePlayerProjection, BasePlayerProjection> OwnerChanged;
        public void ConnectTo(BasePlayerProjection basePlayer)
        {
            var otherOwner = Owner;
            if (otherOwner != null)
            {
                Owner = null;
                if (otherOwner != basePlayer)
                    otherOwner.RemoveOwned(this);
            }

            basePlayer.AddOwned(this);
            Owner = basePlayer;
        }

        public virtual void Replenish()
        {
        }

        public void ConnectTo(int basePlayerID)
        {
            throw new NotImplementedException();
        }
    }
}