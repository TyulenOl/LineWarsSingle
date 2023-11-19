using System;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu(fileName = "PlayerRules", menuName = "Create PlayerRules", order = 56)]
    public class PlayerRules : ScriptableObject
    {
        [field: SerializeField] public int DefaultIncome { get; private set; }
        [field: SerializeField] public int StartMoney { get; private set; }
        [field: SerializeField] public int MoneyForFirstCapturingNode { get; private set; }
        [field: SerializeField] public FloatModifier IncomeModifier { get; private set; }
        [field: SerializeField] public UnitCostFunction CostFunction { get; private set; }
        
        
        private PlayerRules ToDefault()
        {
            DefaultIncome = 2;
            StartMoney = 100;
            MoneyForFirstCapturingNode = 0;
            IncomeModifier = CreateInstance<MultiplyFloatModifier>();
            CostFunction = Resources.Load<AdvancedUnitCostFunction>("Utilities/DefaultCostFunction");
            return this;
        }
        public static PlayerRules DefaultRules => CreateInstance<PlayerRules>().ToDefault();
    }
}