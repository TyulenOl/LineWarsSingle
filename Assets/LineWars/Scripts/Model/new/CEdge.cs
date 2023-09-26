using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    public class CEdge: INumbered, IAlive
    {
        public int Index { get; set; }
        public CNode FirstNode { get; }
        public CNode SecondNode { get; }
        
        private readonly Dictionary<LineType, _LineInfo> lineMap;

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

        public event Action<int, int> HpChanged;
        public event Action<LineType, LineType> LineTypeChanged;

        public CEdge(
            int index,
            LineType lineType,
            CNode firstNode,
            CNode secondNode,
            Dictionary<LineType, LineTypeCharacteristics> lineTypeCharacteristicsMap)
        {
            Index = index;
            lineMap = new Dictionary<LineType, _LineInfo>(lineTypeCharacteristicsMap.Count);
            foreach (var (key, value) in lineTypeCharacteristicsMap)
                lineMap.Add(key, new _LineInfo(value.MaxHp));
            
            LineType = lineType;
            FirstNode = firstNode;
            SecondNode = secondNode;
        }

        public void TakeDamage(Hit hit) => CurrentHp -= hit.Damage;
        public CNode GetOther(CNode node) => FirstNode.Equals(node) ? SecondNode : FirstNode;
        public void LevelUp() => LineType = LineTypeHelper.Up(LineType);


        readonly struct _LineInfo
        {
            public readonly int MaxHp;

            public _LineInfo(int maxHp)
            {
                MaxHp = maxHp;
            }
        }
    }
}