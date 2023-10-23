﻿using System;
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
            BasePlayerProjection>> actionsDictionary 
            = new Dictionary<CommandType, UnitAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection>>();
        private IEnumerable<MonoUnitAction> monoActions;

        private IEnumerable<UnitAction<NodeProjection, EdgeProjection,
            UnitProjection, OwnedProjection,
            BasePlayerProjection>> unitActions;

        public Unit Original { get; private set; }
        public string UnitName { get; private set; }
        public int MaxHp { get; private set; }
        public int MaxArmor { get; private set; }
        public int MaxActionPoints { get; private set; }
        public int Visibility { get; private set; }
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
        public event Action<IExecutorAction> CurrentActionCompleted;

        public IReadOnlyDictionary<CommandType, UnitAction<NodeProjection, EdgeProjection,
            UnitProjection, OwnedProjection,
            BasePlayerProjection>> ActionsDictionary => actionsDictionary;
        public bool HasOriginal => Original != null;

        int IReadOnlyExecutor.CurrentActionPoints => CurrentActionPoints;

        public UnitProjection(string unitName, 
            int maxHp, 
            int maxArmor,
            int maxActionPoints,
            int visibility, 
            UnitType type, 
            UnitSize size, 
            LineType lineType,
            CommandPriorityData commandPriorityData,
            int currentArmor, 
            UnitDirection unitDirection, 
            IEnumerable<MonoUnitAction> actions,
            int currentActionPoints,
            int id,
            bool hasId,
            Unit original = null,
            NodeProjection node = null)
        {
            UnitName = unitName;
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
            MaxActionPoints = maxActionPoints;

            Id = id;
            HasId = hasId;
        }

        public UnitProjection(string unitName,
            int maxHp,
            int maxArmor,
            int maxActionPoints,
            int visibility,
            UnitType type,
            UnitSize size,
            LineType lineType,
            CommandPriorityData commandPriorityData,
            int currentArmor,
            UnitDirection unitDirection,
            IEnumerable<UnitAction<NodeProjection, EdgeProjection,
            UnitProjection, OwnedProjection,
            BasePlayerProjection>> actions,
            int currentActionPoints,
            int id,
            bool hasId,
            Unit original = null,
            NodeProjection node = null)
        {
            UnitName = unitName;
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
            MaxActionPoints = maxActionPoints;

            Id = id;
            HasId = hasId;
        }

        public UnitProjection(IReadOnlyUnitProjection unit, NodeProjection node = null)
            : this(unit.UnitName, unit.MaxHp, unit.MaxArmor, unit.MaxActionPoints, unit.Visibility, unit.Type, unit.Size,
                  unit.MovementLineType, unit.CommandPriorityData, unit.CurrentArmor, unit.UnitDirection,
                  unit.ActionsDictionary.Values, unit.CurrentActionPoints, unit.Id, unit.HasId, unit.Original, node)
        {
        }

        public UnitProjection(Unit original, NodeProjection node = null) 
            : this(original.UnitName, original.MaxHp, original.MaxArmor, original.MaxActionPoints, original.Visibility, original.Type, 
                  original.Size, original.MovementLineType, original.CommandPriorityData, original.CurrentArmor, 
                  original.UnitDirection, original.Actions, original.CurrentActionPoints, original.Id, true, original, node)
        {

        }

        public void SetId(int id)
        {
            Id = id;
            HasId = true;
        }

        public void InitializeActions(GraphProjection graphProjection)
        {
            if (monoActions != null)
            {
                InitializeMonoActions(graphProjection);
                return;
            }
            if(unitActions != null)
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
                actionsDictionary[action.GetMyCommandType()] = 
                    visitor.Result;   
            }
        }

        private void InitializeUnitActions(GraphProjection graphProjection)
        {
            foreach (var action in unitActions)
            {
                var visitor = CopyActionVisitor.Create(this, graphProjection);
                action.Accept(visitor);
                actionsDictionary[action.GetMyCommandType()] =
                    visitor.Result;
            }
        }

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

        public bool TryGetCommand(CommandType priorityType, ITarget target, out ICommand command)
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

        public bool TryGetCommand(CommandType priorityType, IReadOnlyTarget target, out ICommand command)
        {
            return TryGetCommand(priorityType, (ITarget)target, out command);
        }

        public IEnumerable<(IReadOnlyTarget, CommandType)> GetAllAvailableTargets()
        {
            throw new NotImplementedException();
        }

        public override void Replenish()
        {
            base.Replenish();
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

        public event Action AnyActionCompleted;
        public event Action<IExecutorAction> CurrentActionCompleted;

        public IReadOnlyDictionary<CommandType, UnitAction<NodeProjection, EdgeProjection,
            UnitProjection, OwnedProjection,
            BasePlayerProjection>> ActionsDictionary { get; }
        public bool HasOriginal => Original != null;
    }
}