// using System;
// using System.Collections.Generic;
// using System.Diagnostics.CodeAnalysis;
// using LineWars.Controllers;
// using UnityEngine;
//
// namespace LineWars.Model
// {
//     public class Engineer: ComponentUnit, IRoadUpper
//     {
//         [field: Header("Engineer Settings")]
//         [field: SerializeField] private IntModifier engineerPointModifier;
//
//         [Header("Sound Settings")] 
//         [SerializeField] private SFXData upRoadSFX;
//
//         public IntModifier EngineerPointModifier => engineerPointModifier;
//
//         protected override void Awake()
//         {
//             base.Awake();
//             if (engineerPointModifier == null)
//             {
//                 engineerPointModifier = DecreaseIntModifier.DecreaseOne;
//                 Debug.LogWarning($"{nameof(engineerPointModifier)} is null on {name}");
//             }
//         }
//
//         public bool CanUpRoad([NotNull] Edge edge) => CanUpRoad(edge, Node);
//
//         public bool CanUpRoad([NotNull] Edge edge, Node node)
//         {
//             if (edge == null) throw new ArgumentNullException(nameof(edge));
//             return node.ContainsEdge(edge)
//                    && LineTypeHelper.CanUp(edge.LineType)
//                    && ActionPointsCondition(engineerPointModifier, CurrentActionPoints);
//         }
//
//         public void UpRoad([NotNull] Edge edge)
//         {
//             if (edge == null) throw new ArgumentNullException(nameof(edge));
//             edge.LevelUp();
//             CurrentActionPoints = engineerPointModifier.Modify(CurrentActionPoints);
//             ActionCompleted.Invoke();
//             SfxManager.Instance.Play(upRoadSFX);
//         }
//
//         protected override IEnumerable<(ITarget, CommandType)> GetAllAvailableTargetsInRange(uint range)
//         {
//             var visibilityEdges = new HashSet<Edge>();
//             foreach (var e in Graph.GetNodesInRange(Node, range))
//             {
//                 foreach (var target in e.GetTargetsWithMe())
//                     yield return (target, UnitsController.Instance.GetCommandTypeBy(this, target));
//                 foreach (var edge in Node.Edges)
//                 {
//                     if (visibilityEdges.Contains(edge))
//                         continue;
//                     visibilityEdges.Add(edge);
//                     yield return (edge, UnitsController.Instance.GetCommandTypeBy(this, edge));
//                 }
//             }
//         }
//     }
// }