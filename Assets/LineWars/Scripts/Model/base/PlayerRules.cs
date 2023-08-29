using System;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu(fileName = "PlayerRules", menuName = "Create PlayerRules", order = 56)]
    public class PlayerRules : ScriptableObject
    {
        [field: SerializeField] public int DefaultIncome { get; private set; }
        [field: SerializeField] public int StartMoney { get; private set; }
        [field: SerializeField] public NationType Nation { get; private set; }
        [field: SerializeField] public int MoneyForFirstCapturingNode { get; private set; }
        [field: SerializeField] public FloatModifier IncomeModifier { get; private set; }
        [field: SerializeField] public IntModifier ScoreForCapturingNodeModifier { get; private set; }

        private void Awake()
        {
            DefaultIncome = 2;
            StartMoney = 100;
            Nation = NationType.Russia;
            MoneyForFirstCapturingNode = 0;
            IncomeModifier = CreateInstance<MultiplyFloatModifier>();
            ScoreForCapturingNodeModifier = CreateInstance<MultiplyIntModifier>();
        }

        public static PlayerRules DefaultRules => CreateInstance<PlayerRules>();
    }
}