using System;
using UnityEngine;

namespace LineWars.Model
{
    //по-идее, можно было реализовать как и экшоны, но в данном случае это лишнее
    //не использовать состояния! это может плохо сказаться на игре
    public abstract class BaseBlessing: ScriptableObject
    {
        [SerializeField] private string blessingName;
        [SerializeField, TextArea] private string blessingDescription;
        [SerializeField] private Sprite icon;
        [SerializeField] private Optional<Money> cost;
        
        protected Player Player => Player.LocalPlayer;
        public abstract event Action Completed;
        public abstract bool CanExecute();
        public abstract void Execute();

        public string Name => blessingName;
        public string Description => blessingDescription;
        public Sprite Icon => icon;
        public Money Cost => cost.Enabled ? cost.Value : new Money(CostType.Gold, -1);
    }
}