using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace LineWars.Model
{
    public class Doctor : Unit
    {
        [field: Header("Doctor Settings")]
        [field: SerializeField] public bool IsMassHeal { get; private set; }
        [field: SerializeField, Min(0)] public int HealingAmount { get; private set; }
        [field: SerializeField] public bool HealLocked { get; private set; }

        [Header("Action Points Settings")]
        [SerializeField] private IntModifier healPointModifier;

        public bool CanHeal([NotNull] Unit target)
        {
            return !HealLocked && OwnerCondition() && SpaceCondition() 
                   && ActionPointsCondition(healPointModifier, CurrentActionPoints);

            bool SpaceCondition()
            {
                var line = Node.GetLine(target.Node);
                return line != null || IsNeighbour(target);
            }

            bool OwnerCondition()
            {
                return target.Owner == Owner;
            }
        }

        public void Heal([NotNull] Unit target)
        {
            target.HealMe(HealingAmount);
            if (IsMassHeal && TryGetNeighbour(out var neighbour))
                neighbour.HealMe(HealingAmount);
            CurrentActionPoints = healPointModifier.Modify(CurrentActionPoints);
        }
    }
}