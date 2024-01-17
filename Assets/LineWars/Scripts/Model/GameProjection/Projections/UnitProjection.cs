using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    public class UnitProjection :
        OwnedProjection,
        IUnit<NodeProjection, EdgeProjection, UnitProjection>,
        IReadOnlyUnitProjection
    {
        private Dictionary<CommandType, UnitAction<NodeProjection, EdgeProjection, UnitProjection>> actionsDictionary =
            new();

        public IEnumerable<IMonoUnitAction<UnitAction<Node, Edge, Unit>>> MonoActions;
        public IEnumerable<UnitAction<NodeProjection, EdgeProjection, UnitProjection>> UnitActions { get; set; }
        public Unit Original { get; set; }
        public string UnitName { get; set; }
        public int InitialPower {get; set; }

        private int currentPower;
        public int CurrentPower 
        {
            get => currentPower; 
            set
            {
                var prevValue = currentPower;
                currentPower = value;
                UnitPowerChanged?.Invoke(this, prevValue, currentPower);
            }
        }
        public int MaxHp { get; set; }
        public int MaxArmor { get; set; }
        public int MaxActionPoints { get; set; }
        public int Visibility { get; set; }
        public UnitType Type { get; set; }
        public UnitSize Size { get; set; }
        public LineType MovementLineType { get; set; }
        public int Id { get; set; }
        public bool HasId { get; set; }
        public CommandPriorityData CommandPriorityData { get; set; }

        private int currentArmor;
        public int CurrentArmor 
        {
            get => currentArmor;
            set
            {
                var prevValue = currentArmor;
                currentArmor = Mathf.Min(0, value);
                UnitArmorChanged?.Invoke(this, prevValue, currentArmor);
            }
        }
        public UnitDirection UnitDirection { get; set; }
        private NodeProjection node;
        public NodeProjection Node
        {
            get => node;
            set
            {
                var prevNode = node;
                node = value;
                UnitNodeChanged?.Invoke(this, prevNode, node);
            }
        }

        private int currentActionPoints;
        public int CurrentActionPoints 
        {
            get => currentActionPoints;
            set
            {

                var prevValue = currentActionPoints;
                currentActionPoints = value;
                UnitActionPointsChanged?.Invoke(this, prevValue, value);
            }
        }

        private int currentHp;

        public int CurrentHp
        {
            get => currentHp;
            set
            {
                var prevValue = currentHp;
                currentHp = value;
                UnitHPChanged?.Invoke(this, prevValue, value);
                if (value <= 0)
                {
                    Died?.Invoke(this);
                    ExecutorDestroyed?.Invoke();
                    RemoveFromNode();
                    RemoveFromOwner();
                }
            }
        }
        public event Action ExecutorDestroyed;
        IEnumerable<IExecutorAction> IExecutor.Actions => actionsDictionary.Values;

        public event Action<UnitProjection> Died;
        public event Action<UnitProjection, NodeProjection, NodeProjection> UnitNodeChanged;
        public event Action<UnitProjection, int, int> UnitHPChanged;
        public event Action<UnitProjection, int, int> UnitActionPointsChanged;
        public event Action<UnitProjection, int, int> UnitPowerChanged;
        public event Action<UnitProjection, int, int> UnitArmorChanged;
        public event Action<UnitProjection> UnitReplenished;

        public IReadOnlyDictionary<CommandType, UnitAction<NodeProjection, EdgeProjection, UnitProjection>>
            ActionsDictionary => actionsDictionary;

        public bool HasOriginal => Original != null;

        public void SetId(int id)
        {
            HasId = true;
            Id = id;
        }

        public void InitializeActions(GraphProjection graphProjection)
        {
            if (MonoActions != null)
            {
                InitializeMonoActions(graphProjection);
                return;
            }

            if (UnitActions != null)
            {
                InitializeUnitActions(graphProjection);
                return;
            }
        }

        private void InitializeMonoActions(GraphProjection graphProjection)
        {
            foreach (var action in MonoActions)
            {
                var visitor = ConvertMonoActionVisitor.Create(this, graphProjection);
                action.Accept(visitor);
                actionsDictionary[action.CommandType] =
                    visitor.Result;
            }
        }

        private void InitializeUnitActions(GraphProjection graphProjection)
        {
            foreach (var action in UnitActions)
            {
                var visitor = CopyActionVisitor.Create(this, graphProjection);
                action.Accept(visitor);
                actionsDictionary[action.CommandType] =
                    visitor.Result;
            }
        }

        private void RemoveFromNode()
        {
            if (Node == null) return;
            if (Node.LeftUnit == this)
                Node.LeftUnit = null;
            if (Node.RightUnit == this)
                Node.RightUnit = null;
        }

        private void RemoveFromOwner()
        {
            Owner.RemoveOwned(this);
        }

        public IEnumerable<IUnitAction<NodeProjection, EdgeProjection, UnitProjection>> Actions =>
            actionsDictionary.Values;

        public IReadOnlyList<Effect<NodeProjection, EdgeProjection, UnitProjection>> Effects => 
            new List<Effect<NodeProjection, EdgeProjection, UnitProjection>>(); //TODO

        public T GetAction<T>()
            where T : IUnitAction<NodeProjection, EdgeProjection, UnitProjection>
        {
            return actionsDictionary.Values.OfType<T>().FirstOrDefault();
        }

        public bool TryGetAction<T>(out T action)
            where T : IUnitAction<NodeProjection, EdgeProjection, UnitProjection>
        {
            action = GetAction<T>();
            return action != null;
        }

        public override void Replenish()
        {
            base.Replenish();
            foreach (var action in actionsDictionary.Values)
            {
                action.OnReplenish();
            }

            CurrentActionPoints = MaxActionPoints;
            UnitReplenished?.Invoke(this);
        }

        public void AddEffect(Effect<NodeProjection, EdgeProjection, UnitProjection> effect)
        {
            //TODO
        }

        public void DeleteEffect(Effect<NodeProjection, EdgeProjection, UnitProjection> effect)
        {
            //TODO
        }
    }

    public interface IReadOnlyUnitProjection : INumbered
    {
        public Unit Original { get; }
        public string UnitName { get; }
        public int InitialPower { get; }
        public int CurrentPower { get; }
        public int MaxHp { get; }
        public int MaxArmor { get; }
        public int MaxActionPoints { get; }
        public int Visibility { get; }
        public UnitType Type { get; }
        public UnitSize Size { get; }
        public LineType MovementLineType { get; }
        public bool HasId { get; }
        public CommandPriorityData CommandPriorityData { get; }
        public bool CanDoAnyAction => CurrentActionPoints > 0;

        public int CurrentArmor { get; }
        public UnitDirection UnitDirection { get; }
        public NodeProjection Node { get; }
        public int CurrentActionPoints { get; }
        public int CurrentHp { get; }

        public IReadOnlyDictionary<CommandType, UnitAction<NodeProjection, EdgeProjection, UnitProjection>>
            ActionsDictionary { get; }

        public bool HasOriginal => Original != null;
    }
}