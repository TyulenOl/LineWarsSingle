﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class BasePlayerProjection :
        IBasePlayer<OwnedProjection,
            BasePlayerProjection>, IReadOnlyBasePlayerProjection
    {
        private NodeProjection baseProjection;
        private HashSet<OwnedProjection> ownedObjects;
        public BasePlayer Original { get; private set; }
        public NodeProjection Base
        {
            get => baseProjection;

            set
            {
                if (value.Owner != this) throw new ArgumentException();

                baseProjection = value;
            }
        }
        public PlayerRules Rules { get; private set; }
        public PhaseExecutorsData PhaseExecutorsData { get; private set; }
        public int Income { get; set; }
        public int CurrentMoney { get; set; }
        public IReadOnlyCollection<OwnedProjection> OwnedObjects => ownedObjects;

        public BasePlayerProjection(PlayerRules rules, 
            int income, int currentMoney, PhaseExecutorsData executorsData, BasePlayer original = null,
            IReadOnlyCollection<OwnedProjection> ownedObjects = null, NodeProjection playerBase = null)
        {
            Original = original;
            Base = playerBase;
            Rules = rules;
            Income = income;
            CurrentMoney = currentMoney;
            PhaseExecutorsData = executorsData;
            if(ownedObjects != null) 
                this.ownedObjects = new HashSet<OwnedProjection>(ownedObjects);
            else 
                this.ownedObjects = new HashSet<OwnedProjection>();
        }

       public BasePlayerProjection(BasePlayer original, IReadOnlyCollection<OwnedProjection> ownedObjects = null)
            : this(original.Rules, original.Income, original.CurrentMoney, original.PhaseExecutorsData, original)
        {
        }

        public BasePlayerProjection(IReadOnlyBasePlayerProjection playerProjection)
             : this(playerProjection.Rules, playerProjection.Income, playerProjection.CurrentMoney,
                   playerProjection.PhaseExecutorsData, playerProjection.Original,
                   playerProjection.OwnedObjects, playerProjection.Base)
        { }

        public void SimulateReplenish()
        {
            foreach(var owned in ownedObjects)
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

            ownedObjects.Add(owned);
        }

        public bool CanSpawnPreset(UnitBuyPreset preset)
        {
            throw new NotImplementedException();
        }

        public void RemoveOwned([NotNull] OwnedProjection owned)
        {
            if (owned == null) throw new ArgumentNullException(nameof(owned));
            if (!ownedObjects.Contains(owned)) return;

            ownedObjects.Remove(owned);
        }

        public void SpawnPreset(UnitBuyPreset preset)
        {
            throw new NotImplementedException();
        }
    }

    public interface IReadOnlyBasePlayerProjection
    {
        public BasePlayer Original { get; }
        public NodeProjection Base { get; }
        public PlayerRules Rules { get; }
        public PhaseExecutorsData PhaseExecutorsData { get; }  
        public IReadOnlyCollection<OwnedProjection> OwnedObjects { get; }
        public int Income { get; }
        public int CurrentMoney { get; }
        public bool HasOriginal => Original != null;
    }
}
