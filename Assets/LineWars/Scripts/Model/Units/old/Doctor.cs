// using System.Diagnostics.CodeAnalysis;
// using LineWars.Controllers;
// using UnityEngine;
//
// namespace LineWars.Model
// {
//     public class Doctor : ComponentUnit, IDoctor
//     {
//         [field: Header("Doctor Settings")]
//         [field: SerializeField] public bool IsMassHeal { get; private set; }
//         [field: SerializeField, Min(0)] public int HealingAmount { get; private set; }
//         [field: SerializeField] public bool HealLocked { get; private set; }
//
//         [Header("Action Points Settings")]
//         [SerializeField] private IntModifier healPointModifier;
//
//         [Header("Sound Settings")] 
//         [SerializeField] private SFXData healSFX;
//         
//
//         public IntModifier HealPointModifier => healPointModifier;
//
//         public bool CanHeal([NotNull] ComponentUnit target)
//         {
//             return !HealLocked 
//                    && OwnerCondition()
//                    && SpaceCondition() 
//                    && ActionPointsCondition(healPointModifier, CurrentActionPoints) 
//                    && target != this 
//                    && target.CurrentHp != target.MaxHp;
//
//             bool SpaceCondition()
//             {
//                 var line = Node.GetLine(target.Node);
//                 return line != null || IsNeighbour(target);
//             }
//
//             bool OwnerCondition()
//             {
//                 return target.Owner == Owner;
//             }
//         }
//
//         public void Heal([NotNull] ComponentUnit target)
//         {
//             target.HealMe(HealingAmount);
//             if (IsMassHeal && TryGetNeighbour(out var neighbour))
//                 neighbour.HealMe(HealingAmount);
//             CurrentActionPoints = healPointModifier.Modify(CurrentActionPoints);
//             ActionCompleted.Invoke();
//             SfxManager.Instance.Play(healSFX);
//         }
//     }
// }