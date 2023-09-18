﻿// using System.Collections.Generic;
// using LineWars.Controllers;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace LineWars.Model
// {
//     public class DistanceAttackUnit : ComponentUnit
//     {
//         [field: Header("Distance Attack Settings")]
//         [field: SerializeField, Min(0)] public int Distance { get; private set; }
//
//         public override bool CanAttack(ComponentUnit unit) => CanAttack(unit, Node);
//
//         public override bool CanAttack(ComponentUnit unit, Node node)
//         {
//             return !attackLocked
//                    && Damage > 0
//                    && unit.Owner != Owner
//                    && node.FindShortestPath(unit.Node).Count - 1 <= Distance
//                    && ActionPointsCondition(attackPointsModifier, CurrentActionPoints);
//         }
//
//         public override void Attack(ComponentUnit enemy)
//         {
//             DistanceAttack(enemy, Damage);
//             SfxManager.Instance.Play(attackSFX);
//         }
//
//         protected void DistanceAttack(IAlive alive, int damage)
//         {
//             alive.TakeDamage(new Hit(damage, this, alive, isPenetratingDamage, true));
//             CurrentActionPoints = attackPointsModifier.Modify(CurrentActionPoints);
//         
//             ActionCompleted.Invoke();
//         }
//
//         public override CommandType GetAttackTypeBy(IAlive target)
//         {
//             return CommandType.Fire;
//         }
//         
//         public override IEnumerable<(ITarget, CommandType)> GetAllAvailableTargets()
//         {
//             return GetAllAvailableTargetsInRange((uint)Distance + 1);
//         }
//     }
// }