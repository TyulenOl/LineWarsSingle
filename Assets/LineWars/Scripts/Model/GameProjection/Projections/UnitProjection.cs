using System;
using System.Collections.Generic;
using System.Linq;


namespace LineWars.Model
{
    public class UnitProjection
        : OwnedProjection, IUnit<NodeProjection, EdgeProjection,
            UnitProjection, OwnedProjection,
            BasePlayerProjection>, IReadOnlyUnitProjection
    {
        private Dictionary<CommandType, UnitAction<NodeProjection, EdgeProjection,
            UnitProjection, OwnedProjection,
            BasePlayerProjection>> actionsDictionary = new();

        private IEnumerable<IMonoUnitAction<UnitAction<Node, Edge, Unit, Owned, BasePlayer>>> monoActions;

        private IEnumerable<UnitAction<NodeProjection, EdgeProjection,
            UnitProjection, OwnedProjection,
            BasePlayerProjection>> unitActions;

        public Unit Original { get; private set; }
        public string UnitName { get; private set; }
        public int MaxHp { get; set; }
        public int MaxArmor { get; set; }
        public int MaxActionPoints { get; set; }
        public int Visibility { get; set; }
        public UnitType Type { get; private set; }
        public UnitSize Size { get; private set; }
        public LineType MovementLineType { get; private set; }
        public int Id { get; private set; }
        public bool HasId { get; private set; }
        public CommandPriorityData CommandPriorityData { get; private set; }

        public int CurrentArmor { get; set; }
        public UnitDirection UnitDirection { get; set; }
        public NodeProjection Node { get; set; }
        public int CurrentActionPoints { get; set; }
        public int CurrentHp { get; set; }

        public event Action AnyActionCompleted;

        public IReadOnlyDictionary<CommandType, UnitAction<NodeProjection, EdgeProjection,
            UnitProjection, OwnedProjection,
            BasePlayerProjection>> ActionsDictionary => actionsDictionary;

        public bool HasOriginal => Original != null;

        public UnitProjection(string unitName,
            int currentHp,
            int maxHp,
            int maxArmor,
            int visibility,
            UnitType type,
            UnitSize size,
            LineType lineType,
            CommandPriorityData commandPriorityData,
            int currentArmor,
            UnitDirection unitDirection,
            IEnumerable<IMonoUnitAction<UnitAction<Node, Edge, Unit, Owned, BasePlayer>>> actions,
            int currentActionPoints,
            bool hasId,
            int id,
            Unit original = null,
            NodeProjection node = null)
        {
            UnitName = unitName;
            CurrentHp = currentHp;
            MaxHp = maxHp;
            MaxArmor = maxArmor;
            Visibility = visibility;
            Type = type;
            Size = size;
            MovementLineType = lineType;
            CommandPriorityData = commandPriorityData;
            CurrentArmor = currentArmor;
            UnitDirection = unitDirection;
            Node = node;
            CurrentActionPoints = currentActionPoints;
            Original = original;
            monoActions = actions;


            HasId = hasId;
            Id = id;
        }

        public UnitProjection(
            string unitName,
            int currentHp,
            int maxHp,
            int maxArmor,
            int visibility,
            UnitType type,
            UnitSize size,
            LineType lineType,
            CommandPriorityData commandPriorityData,
            int currentArmor,
            UnitDirection unitDirection,
            IEnumerable<UnitAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection>> actions,
            int currentActionPoints,
            bool hasId,
            int id,
            Unit original = null,
            NodeProjection node = null)
        {
            UnitName = unitName;
            CurrentHp = currentHp;
            MaxHp = maxHp;
            MaxArmor = maxArmor;
            Visibility = visibility;
            Type = type;
            Size = size;
            MovementLineType = lineType;
            CommandPriorityData = commandPriorityData;
            CurrentArmor = currentArmor;
            UnitDirection = unitDirection;
            Node = node;
            CurrentActionPoints = currentActionPoints;
            Original = original;
            unitActions = actions;

            HasId = hasId;
            Id = id;
        }

        public UnitProjection(IReadOnlyUnitProjection unit, NodeProjection node = null)
            : this(unit.UnitName, unit.CurrentHp, unit.MaxHp, unit.MaxArmor, unit.Visibility, unit.Type, unit.Size,
                unit.MovementLineType, unit.CommandPriorityData, unit.CurrentArmor, unit.UnitDirection,
                unit.ActionsDictionary.Values, unit.CurrentActionPoints, true, unit.Id, unit.Original, node)
        {
        }

        public UnitProjection(Unit original, NodeProjection node = null) : this(
             unitName: original.UnitName,
             currentHp : original.CurrentHp,
             maxHp: original.MaxHp,
             maxArmor: original.MaxArmor,
             visibility: original.Visibility,
             type: original.Type,
             size: original.Size,
             lineType: original.MovementLineType,
             commandPriorityData: original.CommandPriorityData,
             currentArmor: original.CurrentArmor,
             unitDirection: original.UnitDirection,
             actions: original.MonoActions,
             currentActionPoints: original.CurrentActionPoints,
             hasId: true,
             id: original.Id,
             original: original,
             node: node)
        {
        }

        public void SetId(int id)
        {
            HasId = true;
            Id = id;
        }

        public void InitializeActions(GraphProjection graphProjection)
        {
            if (monoActions != null)
            {
                InitializeMonoActions(graphProjection);
                return;
            }

            if (unitActions != null)
            {
                InitializeUnitActions(graphProjection);
                return;
            }
        }

        private void InitializeMonoActions(GraphProjection graphProjection)
        {
            foreach (var action in monoActions)
            {
                var visitor = ConvertMonoActionVisitor.Create(this, graphProjection);
                action.Accept(visitor);
                actionsDictionary[action.CommandType] =
                    visitor.Result;
            }
        }

        private void InitializeUnitActions(GraphProjection graphProjection)
        {
            foreach (var action in unitActions)
            {
                var visitor = CopyActionVisitor.Create(this, graphProjection);
                action.Accept(visitor);
                actionsDictionary[action.CommandType] =
                    visitor.Result;
            }
        }

        public IEnumerable<IUnitAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection,
            BasePlayerProjection>> Actions => actionsDictionary.Values;

        public T GetUnitAction<T>()
            where T : IUnitAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection>
        {
            return actionsDictionary.Values.OfType<T>().FirstOrDefault();
        }

        public bool TryGetUnitAction<T>(out T action)
            where T : IUnitAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection>
        {
            action = GetUnitAction<T>();
            return action != null;
        }

        public bool TryGetCommandForTarget(CommandType priorityType, ITarget target,
            out ICommandWithCommandType command)
        {
            if (actionsDictionary.TryGetValue(priorityType, out var value)
                && value is ITargetedAction targetedAction
                && targetedAction.IsMyTarget(target))
            {
                command = targetedAction.GenerateCommand(target);
                return true;
            }

            command = null;
            return false;
        }

        public override void Replenish()
        {
            base.Replenish();
            CurrentActionPoints = MaxActionPoints;
        }

        public T Accept<T>(IExecutorVisitor<T> visitor) => visitor.Visit(this);
    }

    public interface IReadOnlyUnitProjection
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
        public int Id { get; }
        public CommandPriorityData CommandPriorityData { get; }
        public bool CanDoAnyAction => CurrentActionPoints > 0;

        public int CurrentArmor { get; }
        public UnitDirection UnitDirection { get; }
        public NodeProjection Node { get; }
        public int CurrentActionPoints { get; }
        public int CurrentHp { get; }
        public event Action AnyActionCompleted;

        public IReadOnlyDictionary<CommandType, UnitAction<NodeProjection, EdgeProjection,
            UnitProjection, OwnedProjection,
            BasePlayerProjection>> ActionsDictionary { get; }

        public bool HasOriginal => Original != null;
    }
}