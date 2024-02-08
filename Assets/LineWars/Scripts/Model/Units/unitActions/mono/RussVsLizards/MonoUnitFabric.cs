using System;

namespace LineWars.Model
{
    public class MonoUnitFabric :
        IUnitFabric<Node, Edge, Unit>
    {
        private Lazy<BasePlayer> _player;
        private BasePlayer player => _player.Value;
        private Unit spawnUnit;

        public MonoUnitFabric(Lazy<BasePlayer> player, Unit spawnUnit)
        {
            _player = player;
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
