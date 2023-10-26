// using System;
//
// namespace LineWars.Model
// {
//     public class ContrAttackCommandBlueprint : ICommandBlueprint
//     {
//         public int UnitId { get; private set; }
//         public int TargetId { get; private set; }
//
//         public ContrAttackCommandBlueprint(int unitId, int targetId)
//         {
//             UnitId = unitId;
//             TargetId = targetId;
//         }
//
//         public ICommand GenerateCommand(GameProjection projection)
//         {
//             var unit = projection.UnitsIndexList[UnitId];
//             var targetUnit = projection.UnitsIndexList[TargetId];
//             return new ContrAttackCommand
//                 <NodeProjection, EdgeProjection, UnitProjection,
//                 OwnedProjection, BasePlayerProjection>(unit, targetUnit);
//         }
//
//         public ICommand GenerateMonoCommand(GameProjection projection)
//         {
//             var unit = projection.UnitsIndexList[UnitId].Original;
//             var targetUnit = projection.UnitsIndexList[TargetId].Original;
//             return new BlockAttackCommand
//                 <Node, Edge, Unit,
//                 Owned, BasePlayer>(unit, targetUnit);
//         }
//     }
// }
