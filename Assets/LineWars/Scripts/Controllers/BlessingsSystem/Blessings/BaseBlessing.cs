using System;
using UnityEngine;

namespace LineWars.Model
{
    //по-идее, можно было реализовать как и экшоны, но в данном случае это лишнее
    //не использовать состояния! это может плохо сказаться на игре
    public abstract class BaseBlessing: ScriptableObject
    {
        [SerializeField] private Optional<string> overrideName;
        [SerializeField, TextArea] private Optional<string> overrideDescription;
        [SerializeField] private Sprite icon;
        [SerializeField] private Optional<Money> cost;
        
        protected Player Player => Player.LocalPlayer;
        public abstract event Action Completed;
        public abstract bool CanExecute();
        public abstract void Execute();

        protected virtual string DefaultName { get; } = "Привет из мира багов!";
        protected virtual string DefaultDescription { get; } = "Привет из мира багов!";
        public string Name => overrideName.Enabled ? overrideName.Value : DefaultName;
        public string Description => overrideDescription.Enabled ? overrideDescription.Value : DefaultDescription;
        public Sprite Icon => icon;
        public Money Cost => cost.Enabled ? cost.Value : new Money(CostType.Gold, -1);
    }
}