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
        public int Id { get; private set; }
        public BasePlayer Original { get; private set; }
        public GameProjection Game { get; set; }
        public NodeProjection Base
        {
            get => baseProjection;

            set
            {
                if(value == null)
                {
                    baseProjection = value;
                    return;
                }

                if (value.Owner != this) throw new ArgumentException();

                baseProjection = value;
            }
        }
        public PlayerRules Rules { get; private set; }
        public PhaseExecutorsData PhaseExecutorsData { get; private set; }
        public NationEconomicLogic EconomicLogic { get; private set; } 
        public int Income { get; set; }
        public int CurrentMoney { get; set; }
        public IReadOnlyCollection<OwnedProjection> OwnedObjects => ownedObjects;

        public BasePlayerProjection(PlayerRules rules, 
            int income, int currentMoney, PhaseExecutorsData executorsData, NationEconomicLogic economicLogic, int id, BasePlayer original = null,
            IReadOnlyCollection<OwnedProjection> ownedObjects = null, NodeProjection playerBase = null)
        {
            Original = original;
            Base = playerBase;
            Rules = rules;
            Income = income;
            CurrentMoney = currentMoney;
            PhaseExecutorsData = executorsData;
            EconomicLogic = economicLogic;
            Id = id;
            if(ownedObjects != null) 
                this.ownedObjects = new HashSet<OwnedProjection>(ownedObjects);
            else 
                this.ownedObjects = new HashSet<OwnedProjection>();
        }

       public BasePlayerProjection(BasePlayer original, IReadOnlyCollection<OwnedProjection> ownedObjects = null, NodeProjection playerBase = null)
            : this(original.Rules, original.Income, original.CurrentMoney, original.PhaseExecutorsData, original.EconomicLogic, original.Id, original, ownedObjects, playerBase)
        {
        }

        public BasePlayerProjection(IReadOnlyBasePlayerProjection playerProjection, IReadOnlyCollection<OwnedProjection> ownedObjects = null, NodeProjection playerBase = null)
             : this(playerProjection.Rules, playerProjection.Income, playerProjection.CurrentMoney,
                   playerProjection.PhaseExecutorsData, playerProjection.EconomicLogic, playerProjection.Id, playerProjection.Original,
                   ownedObjects, playerBase)
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
            return Base.LeftUnit == null && Base.RightUnit == null;
        }

        public void RemoveOwned([NotNull] OwnedProjection owned)
        {
            if (owned == null) throw new ArgumentNullException(nameof(owned));
            if (!ownedObjects.Contains(owned)) return;

            ownedObjects.Remove(owned);
        }

        public void SpawnPreset(UnitBuyPreset preset)
        {
            var leftUnit = preset.FirstUnitType;
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