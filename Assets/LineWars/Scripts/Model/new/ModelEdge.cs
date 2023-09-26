using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace LineWars.Model
{
    public class ModelEdge: IAlive, ITarget, IHavePosition
    {
        public int Index { get; }
        public ModelNode FirstNode { get; }
        public ModelNode SecondNode { get; }

        public Vector2 Position { get; }

        private readonly Dictionary<LineType, LineInfo> lineMap;

        public int MaxHp => lineMap.ContainsKey(LineType)
            ? lineMap[LineType].MaxHp
            : 0;

        private int hp;

        public int CurrentHp
        {
            get => hp;
            private set
            {
                var before = hp;

                if (value < 0)
                {
                    LineType = LineTypeHelper.Down(LineType);
                    hp = MaxHp;
                }
                else
                    hp = Math.Min(value, MaxHp);

                HpChanged?.Invoke(before, hp);
            }
        }

        private LineType lineType;

        public LineType LineType
        {
            get => lineType;
            private set
            {
                var before = lineType;
                lineType = value;
                LineTypeChanged?.Invoke(before, lineType);
                CurrentHp = MaxHp;
            }
        }

        public CommandPriorityData CommandPriorityData { get; }

        public event Action<int, int> HpChanged;
        public event Action<LineType, LineType> LineTypeChanged;

        public ModelEdge(
            int index,
            Vector2 position,
            LineType lineType,
            [NotNull] ModelNode firstNode,
            [NotNull] ModelNode secondNode,
            [NotNull] CommandPriorityData commandPriorityData,
            [NotNull] Dictionary<LineType, LineTypeCharacteristics> lineTypeCharacteristicsMap)
        {
            if (firstNode == null) throw new ArgumentNullException(nameof(firstNode));
            if (secondNode == null) throw new ArgumentNullException(nameof(secondNode));
            if (commandPriorityData == null) throw new ArgumentNullException(nameof(commandPriorityData));
            if (lineTypeCharacteristicsMap == null) throw new ArgumentNullException(nameof(lineTypeCharacteristicsMap));
            
            
            Index = index;
            Position = position;
            lineMap = new Dictionary<LineType, LineInfo>(lineTypeCharacteristicsMap.Count);
            foreach (var (key, value) in lineTypeCharacteristicsMap)
                lineMap.Add(key, new LineInfo(value.MaxHp));
            
            LineType = lineType;
            FirstNode = firstNode;
            SecondNode = secondNode;
            CommandPriorityData = commandPriorityData;
        }

        public void TakeDamage(Hit hit) => CurrentHp -= hit.Damage;
        public ModelNode GetOther(ModelNode node) => FirstNode.Equals(node) ? SecondNode : FirstNode;
        public void LevelUp() => LineType = LineTypeHelper.Up(LineType);


        readonly struct LineInfo
        {
            public readonly int MaxHp;

            public LineInfo(int maxHp)
            {
                MaxHp = maxHp;
            }
        }
    }
}