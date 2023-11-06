using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    public static class BasePlayerUtility
    {
        public static bool CanSpawnUnit(Node node, Unit unit, UnitDirection unitDirection = UnitDirection.Any)
        {
            return node != null && unit != null &&
                   (unit.Size == UnitSize.Large && node.LeftIsFree && node.RightIsFree 
                    || unit.Size == UnitSize.Little && (
                        unitDirection is UnitDirection.Left or UnitDirection.Any && node.LeftIsFree
                        || unitDirection is UnitDirection.Right or UnitDirection.Any && node.RightIsFree
                    )
                   );
        }

        public static Unit CreateUnitForPlayer(
            BasePlayer player,
            Node node,
            Unit unitPrefab,
            UnitDirection unitDirection = UnitDirection.Any)
        {
            var unit = Object.Instantiate(unitPrefab, player.transform);
            unit.transform.position = node.transform.position;

            if (unit.Size == UnitSize.Large)
            {
                unit.Initialize(node, UnitDirection.Any);
                node.RightUnit = unit;
                node.LeftUnit = unit;
            }
            else
                switch (unitDirection)
                {
                    case UnitDirection.Left or UnitDirection.Any when node.LeftIsFree:
                        unit.Initialize(node, UnitDirection.Left);
                        node.LeftUnit = unit;
                        break;
                    case UnitDirection.Right or UnitDirection.Any when node.RightIsFree:
                        unit.Initialize(node, UnitDirection.Right);
                        node.RightUnit = unit;
                        break;
                    default:
                        Debug.LogError($"Юнит {nameof(unit)} не может быть создан на {nameof(node)}");
                        break;
                }

            Owned.Connect(player, unit);
            return unit;
        }
        
        public static int GetCountUnitByType(this BasePlayer player, UnitType type)
        {
            return player.OwnedObjects
                .OfType<Unit>()
                .Count(x => x.Type == type);
        }
    }
}