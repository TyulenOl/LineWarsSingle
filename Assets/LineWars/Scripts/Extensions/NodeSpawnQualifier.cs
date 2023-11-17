using System.Linq;

namespace LineWars.Model
{
    public static class NodeSpawnQualifier
    {
        public static bool IsQualifiedForSpawn(this Node node, BasePlayer player)
        {
            return (KnyazCondition(node, player)
                   || InitialSpawnCondition(node, player)) && node.AnyIsFree;
        }

        private static bool InitialSpawnCondition(Node node, BasePlayer player)
        {
            return player.InitialSpawns.Contains(node) && node.Owner == player ;
        }

        private static bool KnyazCondition(Node node, BasePlayer player)
        {
            if (node.Owner != player)
                return false;
            
            var knyazes = player.MyUnits.Where(unit => unit.Type == UnitType.Knyaz)
                .ToList();
            if (knyazes.Count == 0)
                return false;
            var radius = knyazes
                .Max(unit =>
                    unit.TryGetComponent(out KnyazRadius knyazRadius) ? knyazRadius.Radius : 1);
            var elligbleNodes = MonoGraph.Instance.GetNodesInRange(node, (uint)radius)
                .Where(thisNode => HaveElligbleKnyaz(thisNode, node))
                .ToList();
            return elligbleNodes.Count > 0;
        }

        private static bool HaveElligbleKnyaz(Node node, Node startNode)
        {
            var result = !node.LeftIsFree
                && node.LeftUnit.Type == UnitType.Knyaz
                && IsKnyazElligble(node, startNode, node.LeftUnit) || !node.RightIsFree
                && node.RightUnit.Type == UnitType.Knyaz
                && IsKnyazElligble(node, startNode, node.RightUnit);

            return result;
        }

        private static bool IsKnyazElligble(Node node, Node startNode, Unit knyaz)
        {
            var pathLength = MonoGraph.Instance.FindShortestPath(node, startNode);
            var knyazRadius = knyaz.TryGetComponent(out KnyazRadius rComponent) ? rComponent.Radius : 1;
            return pathLength.Count <= knyazRadius;
        }
    }
}