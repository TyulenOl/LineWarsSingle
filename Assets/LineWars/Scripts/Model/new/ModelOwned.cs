using System;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public abstract class ModelOwned
    {
        protected ModelBasePlayer basePlayer;
        
        public ModelBasePlayer Owner => basePlayer;
        public event Action Replenished;
        
        protected ModelOwned(ModelBasePlayer basePlayer)
        {
            this.basePlayer = basePlayer;
        }
        
        public void Replenish()
        {
            OnReplenish();
            Replenished?.Invoke();
        }

        protected virtual void OnReplenish() {}
    }
}