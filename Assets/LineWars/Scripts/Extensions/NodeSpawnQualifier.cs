namespace LineWars.Model
{
    public static class NodeSpawnQualifier
    {
        public static bool IsQualifiedForSpawn(this Node node, BasePlayer player) 
        {
            return CzarCondition(node, player)
                || InitialSpawnCondition(node, player);
        }
        private static bool InitialSpawnCondition(Node node, BasePlayer player)
        {
            return player.InitialSpawns.Contains(node) && node.Owner == player;
        }

        private static bool CzarCondition(Node node, BasePlayer player)
        {
            return false;
        }
    }
}
