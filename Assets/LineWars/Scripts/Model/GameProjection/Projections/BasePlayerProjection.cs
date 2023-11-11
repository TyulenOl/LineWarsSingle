using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class BasePlayerProjection : IBasePlayer, IReadOnlyBasePlayerProjection
    {
        private NodeProjection baseProjection;
        public HashSet<OwnedProjection> OwnedObjects { get; set; }
        public int Id { get; set; }
        public BasePlayer Original { get; set; }
        public GameProjection Game { get; set; }
        public NodeProjection Base
        {
            get => baseProjection;

            set
            {
                if (value == null)
                {
                    baseProjection = null;
                    return;
                }

                if (value.Owner != this) throw new ArgumentException();

                baseProjection = value;
            }
        }
        public PlayerRules Rules { get; set; }
        public PhaseExecutorsData PhaseExecutorsData { get; set; }
        public NationEconomicLogic EconomicLogic { get; set; } 
        public int Income { get; set; }
        public int CurrentMoney { get; set; }

        IReadOnlyCollection<OwnedProjection> IReadOnlyBasePlayerProjection.OwnedObjects => OwnedObjects;

        public void SimulateReplenish()
        {
            foreach(var owned in OwnedObjects)
            {
                owned.Replenish();
            }
        }

        public void AddOwned([NotNull] OwnedProjection owned)
        {
            if (owned == null) throw new ArgumentNullException(nameof(owned));
            if (owned.Owner != null && owned.Owner == this) return;
            if (owned.Owner != null && owned.Owner != this)
                owned.Owner.RemoveOwned(owned);

            CheckOwnedValidity(owned);
            OwnedObjects.Add(owned);
            
        }

        private void CheckOwnedValidity(OwnedProjection newProj)
        {
            if(newProj is UnitProjection newUnit)
            {
                foreach(var oldUnit in OwnedObjects
                            .OfType<UnitProjection>())
                {
                    if (newUnit.Id == oldUnit.Id)
                        throw new InvalidOperationException();
                }
            }

            if(newProj is NodeProjection newNode)
            {
                foreach(var oldNode in OwnedObjects
                            .OfType<NodeProjection>())
                {
                    if(newNode.Id == oldNode.Id)
                        throw new InvalidOperationException();
                }
            }
        }

        public bool CanBuyPreset(UnitBuyPreset preset)
        {
            return Base.LeftUnit == null && Base.RightUnit == null;
        }

        public void RemoveOwned([NotNull] OwnedProjection owned)
        {
            if (owned == null) throw new ArgumentNullException(nameof(owned));
            if (!OwnedObjects.Contains(owned)) return;

            OwnedObjects.Remove(owned);
        }

        public void BuyPreset(UnitBuyPreset preset)
        {
            var leftUnit = preset.FirstUnitType; // СПРОСИТЬ У ПАШИ
        }
    }

    public interface IReadOnlyBasePlayerProjection : INumbered
    {
        public BasePlayer Original { get; }
        public NodeProjection Base { get; }
        public PlayerRules Rules { get; }
        public PhaseExecutorsData PhaseExecutorsData { get; }  
        public NationEconomicLogic EconomicLogic { get; }
        public IReadOnlyCollection<OwnedProjection> OwnedObjects { get; }
        public int Income { get; }
        public int CurrentMoney { get; }
        public bool HasOriginal => Original != null;
    }
}
