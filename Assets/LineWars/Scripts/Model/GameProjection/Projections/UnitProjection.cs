using System;
using System.Collections.Generic;
using System.Linq;


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

        public int CurrentArmor { get; set; }
        public UnitDirection UnitDirection { get; set; }
        public NodeProjection Node { get; set; }
        public int CurrentActionPoints { get; set; }

        private int currentHp;

        public int CurrentHp
        {
            get => currentHp;
            set
            {
                currentHp = value;
                if (value == 0)
                {
                    Died?.Invoke(this);
                    RemoveFromNode();
                    RemoveFromOwner();
                }
            }
        }

        public event Action AnyActionCompleted;
        public event Action<UnitProjection> Died;

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

        IEnumerable<IExecutorAction> IExecutorActionSource.Actions => actionsDictionary.Values;

        public T GetUnitAction<T>()
            where T : IUnitAction<NodeProjection, EdgeProjection, UnitProjection>
        {
            return actionsDictionary.Values.OfType<T>().FirstOrDefault();
        }

        public bool TryGetUnitAction<T>(out T action)
            where T : IUnitAction<NodeProjection, EdgeProjection, UnitProjection>
        {
            action = GetUnitAction<T>();
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
        }
    }

    public interface IReadOnlyUnitProjection : INumbered
    {
        public Unit Original { get; }
        public string UnitName { get; }
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