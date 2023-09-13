using System;
using JetBrains.Annotations;
using UnityEngine;

namespace LineWars.Model
{
    public abstract class UnitAction: ScriptableObject
    {
        [SerializeField] private IntModifier actionModifier;
        
        protected CombinedUnit MyUnit;
        public event Action ActionCompleted;
        
        public void Initialize([NotNull] CombinedUnit unit)
        {
            MyUnit = unit ? unit : throw new ArgumentNullException(nameof(unit));
        }
        public abstract CommandType GetMyCommandType();

        public virtual void OnReplenish() {}
        
        protected void Complete() => ActionCompleted?.Invoke();
        
        
        public int ModifyActionPoints(int actionPoints) => actionModifier.Modify(actionPoints);
        public int ModifyActionPoints() => ModifyActionPoints(MyUnit.CurrentActionPoints);
        
        public bool ActionPointsCondition(int actionPoints) => ActionPointsCondition(actionModifier, actionPoints);
        public bool ActionPointsCondition() => ActionPointsCondition(actionModifier, MyUnit.CurrentActionPoints);
        
        public static bool ActionPointsCondition(IntModifier modifier, int actionPoints) =>
            actionPoints > 0 && modifier != null && modifier.Modify(actionPoints) >= 0;
    }
}