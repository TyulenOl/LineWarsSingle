using System;

namespace LineWars.Model
{
    public class MonoUnitFabric :
        IUnitFabric<Node, Edge, Unit>
    {
        private BasePlayer player;
        private Unit spawnUnit;

        public MonoUnitFabric(BasePlayer player, Unit spawnUnit)
        {
            this.player = player;
            this.spawnUnit = spawnUnit;
        }

        public bool CanSpawn(Node node)
        {
            return player.CanSpawnUnit(node, spawnUnit);
        }

        public Unit Spawn(Node node)
        {
            return player.SpawnUnit(node, spawnUnit);
        }
    }
}
