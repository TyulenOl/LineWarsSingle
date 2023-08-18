using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace LineWars.Model
{
    public class Doctor : Unit, IDoctor
    {
        [field: SerializeField] public bool IsMassHeal { get; private set; }
        [field: SerializeField, Min(0)] public int HealingAmount { get; private set; }
        [field: SerializeField] public bool HealLocked { get; set; }

        [Header("Action Points Settings")]
        [SerializeField] private ActionPointsModifier healPointModifier;

        public bool CanHeal([NotNull] Unit target)
        {
            return !HealLocked && OwnerCondition() && SpaceCondition();

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
            target.Heal(HealingAmount);
            if (IsMassHeal && TryGetNeighbour(out var neighbour))
                neighbour.Heal(HealingAmount);
            CurrentActionPoints = healPointModifier.Modify(CurrentActionPoints);
        }
    }
}