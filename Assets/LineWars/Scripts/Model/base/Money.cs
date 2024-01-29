
using System;
using UnityEngine;

namespace LineWars.Model
{
    [Serializable]
    public struct Money
    {
        [SerializeField] private CostType type;
        [SerializeField] private int amount;
        
        public CostType Type => type;
        public int Amount => amount;

        public Money(CostType type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }
    }
}